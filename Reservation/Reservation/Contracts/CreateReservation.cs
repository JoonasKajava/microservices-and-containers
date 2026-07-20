using System.ComponentModel.DataAnnotations;

namespace Reservation.Contracts;

public class CreateReservation
{
    [Required]
    public required Guid EquipmentId { get; set; }

    [Required]
    public DateTimeOffset StartTime { get; set; }

    [Required]
    public DateTimeOffset EndTime { get; set; }
}