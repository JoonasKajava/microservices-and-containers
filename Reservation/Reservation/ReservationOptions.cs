using System.ComponentModel.DataAnnotations;

namespace Reservation;

public class ReservationOptions
{
    public static string SectionName => "Reservation";

    [Required]
    public required string InventorySubAddr { get; set; }

}