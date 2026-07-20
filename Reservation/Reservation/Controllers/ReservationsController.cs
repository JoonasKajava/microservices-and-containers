using Microsoft.AspNetCore.Mvc;
using Reservation.Context;
using Reservation.Contracts;

namespace Reservation.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class ReservationsController(ReservationDbContext dbContext, ILogger<ReservationsController> logger) : ControllerBase
{

    [HttpGet(Name = "GetEquipmentReservations")]
    public IEnumerable<Entities.Reservation> Get([FromQuery] GetEquipmentReservations getEquipmentReservations)
    {
        logger.LogInformation("Getting equipment reservations for {id}", getEquipmentReservations.EquipmentId);
        return dbContext.Reservations;
    }
}
