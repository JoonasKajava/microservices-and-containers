using System.ComponentModel.DataAnnotations;

namespace Reservation;

public class ReservationOptions
{
    public static string SectionName => "Reservation";

    [Required]
    public required string DatabaseUrl { get; set; }


    [Required]
    public required string InventoryServiceUrl { get; set; }

    [Required]
    public required string InventorySubAddr { get; set; }

    [Required]
    public required string JwtAuthority { get; set; }

    [Required]
    public required string JwtAudience { get; set; }

    public string PublisherBindAddr { get; set; } = "tcp://0.0.0.0:5557";
}