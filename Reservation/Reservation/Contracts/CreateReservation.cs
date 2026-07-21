using System.ComponentModel.DataAnnotations;

namespace Reservation.Contracts;

public class CreateReservation
{
    [Required]
    public required Guid EquipmentId { get; set; }

    [Required]
    public required DateTimeOffset StartTime { get; set; }

    [Required]
    public required DateTimeOffset EndTime { get; set; }
}