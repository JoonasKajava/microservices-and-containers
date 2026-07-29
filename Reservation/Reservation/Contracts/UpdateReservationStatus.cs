using System.ComponentModel.DataAnnotations;
using Reservation.Entities;

namespace Reservation.Contracts;

public class UpdateReservationStatus
{
    [Required]
    public required Guid Id { get; set; }

    [Required]
    public required ReservationStatus Status { get; set; }
}