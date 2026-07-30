using System.ComponentModel.DataAnnotations;

namespace Reservation.Contracts;

public class GetEquipmentReservations
{
    [Required]
    public Guid EquipmentId { get; set; }
}