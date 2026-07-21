using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservation.Context;
using Reservation.Contracts;

namespace Reservation.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class ReservationsController(ReservationDbContext dbContext, ILogger<ReservationsController> logger)
    : ControllerBase
{
    [HttpGet(Name = "GetEquipmentReservations")]
    public IEnumerable<Entities.Reservation> Get([FromQuery] GetEquipmentReservations getEquipmentReservations)
    {
        logger.LogInformation("Getting equipment reservations for {id}", getEquipmentReservations.EquipmentId);
        return dbContext.Reservations;
    }

    [HttpPost(Name = "CreateReservation")]
    public async Task<IActionResult> Post([FromBody] CreateReservation createReservation)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // TODO: Check that equipment exists
        // TODO: Check overlapping reservations

        var reservation = new Entities.Reservation
        {
            EquipmentId = createReservation.EquipmentId,
            StartTime = createReservation.StartTime,
            EndTime = createReservation.EndTime
        };

        dbContext.Reservations.Add(reservation);

        await dbContext.SaveChangesAsync();

        return CreatedAtRoute("CreateReservation", reservation);
    }

    [HttpDelete("{id}", Name = "DeleteReservation")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await dbContext.Reservations.Where(e => e.ReservationId == id).ExecuteDeleteAsync();
        return NoContent();
    }
}