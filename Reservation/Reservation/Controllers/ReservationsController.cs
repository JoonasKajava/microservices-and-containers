using Microsoft.AspNetCore.Mvc;
using Reservation.Contracts;
using Reservation.Repositories;

namespace Reservation.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class ReservationsController(
    ILogger<ReservationsController> logger,
    IReservationsRepository reservationsRepository,
    IInventoryRepository inventoryRepository
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

        var equipment = await inventoryRepository.GetEquipmentByIdAsync(createReservation.EquipmentId);

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


        var reservation = new Entities.Reservation
        {
            EquipmentId = createReservation.EquipmentId,
            StartTime = createReservation.StartTime,
            EndTime = createReservation.EndTime
        };

        await reservationsRepository.CreateReservationAsync(reservation);

        return CreatedAtRoute("CreateReservation", reservation);
    }

    [HttpDelete("{id}", Name = "DeleteReservation")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await reservationsRepository.DeleteReservationAsync(id);
        return NoContent();
    }
}