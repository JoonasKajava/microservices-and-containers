using System.ComponentModel.DataAnnotations;

namespace Inventory;

public class InventoryOptions
{
    public static string SectionName => "Inventory";

    public string InventoryPublisherBindAddr { get; set; } = "tcp://0.0.0.0:5556";

    [Required]
    public required string JwtAuthority { get; set; }

    [Required]
    public required string JwtAudience { get; set; }

    [Required]
    public required string ReservationSubAddr { get; set; }
}