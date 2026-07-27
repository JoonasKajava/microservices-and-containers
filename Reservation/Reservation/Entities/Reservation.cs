using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reservation.Entities;

public class Reservation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ReservationId { get; set; }

    [Required]
    public required Guid EquipmentId { get; set; }

    [Required]
    public DateTimeOffset StartTime { get; set; }

    [Required]
    public DateTimeOffset EndTime { get; set; }

    [Required]
    [MaxLength(255)]
    public required string ReservedBy { get; set; }
}