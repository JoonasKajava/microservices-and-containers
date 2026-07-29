using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reservation.Contracts;
using Reservation.Entities;
using Reservation.Repositories;

namespace Reservation.Controllers;

[ApiController]
[Authorize]
[Route("/api/v1/[controller]")]
public class ReservationsController(
    ILogger<ReservationsController> logger,
    IReservationsRepository reservationsRepository,
    IInventoryRepository inventoryRepository,
    IReservationPublisher reservationPublisher
)
    : ControllerBase
{
    [HttpGet(Name = "GetEquipmentReservations")]
    public IEnumerable<Entities.Reservation> Get([FromQuery] GetEquipmentReservations getEquipmentReservations)
    {
        logger.LogInformation("Getting equipment reservations for {id}", getEquipmentReservations.EquipmentId);
        return reservationsRepository.GetReservationsForEquipment(getEquipmentReservations.EquipmentId);
    }

    [HttpPost(Name = "CreateReservation")]
    public async Task<IActionResult> Post([FromBody] CreateReservation createReservation)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var token = HttpContext.Request.Headers["Authorization"].ToString();

        var equipment = await inventoryRepository.GetEquipmentByIdAsync(createReservation.EquipmentId, token);

        if (equipment is null)
        {
            return NotFound($"Equipment with id {createReservation.EquipmentId} not found");
        }

        var hasOverlappingReservations = reservationsRepository.GetOverlappingReservationsForEquipment(
            createReservation.EquipmentId,
            createReservation.StartTime,
            createReservation.EndTime
        ).Any();

        if (hasOverlappingReservations)
        {
            return Conflict();
        }

        var sub = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

        var reservation = new Entities.Reservation
        {
            Status = ReservationStatus.Reserved,
            EquipmentId = createReservation.EquipmentId,
            StartTime = createReservation.StartTime,
            EndTime = createReservation.EndTime,
            ReservedBy = sub!
        };

        await reservationsRepository.CreateReservationAsync(reservation);

        await reservationPublisher.ReservationCreated(reservation);

        return CreatedAtRoute("CreateReservation", reservation);
    }

    [HttpDelete("{id}", Name = "DeleteReservation")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var reservation = await reservationsRepository.GetReservation(id);

        if (reservation is null)
        {
            logger.LogWarning("Reservation with id: {id} not found during delete", id);
            return NotFound($"Reservation with id: {id} not found");
        }

        await reservationsRepository.DeleteReservationAsync(id);

        await reservationPublisher.ReservationCanceled(reservation.ReservationId, reservation.EquipmentId);

        return NoContent();
    }

    [HttpPatch(Name = "UpdateReservation")]
    public async Task<IActionResult> Patch([FromBody] UpdateReservationStatus updateReservationStatus)
    {
        logger.LogInformation("Updating reservation status of {id} to {status}", updateReservationStatus.Id, updateReservationStatus.Status);

        var reservation = await reservationsRepository.UpdateReservationAsync(updateReservationStatus.Id,
            (reservation) => { reservation.Status = updateReservationStatus.Status; });

        switch (updateReservationStatus.Status)
        {
            case ReservationStatus.Started:
                await reservationPublisher.ReservationStarted(reservation);
                break;
            case ReservationStatus.Returned:
                await reservationPublisher.ReservationReturned(reservation);
                break;
        }

        return Ok(reservation);
    }
}