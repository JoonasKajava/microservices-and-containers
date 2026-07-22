using System.ComponentModel.DataAnnotations;

namespace Inventory;

public class InventoryOptions
{
    public static string SectionName => "Inventory";

    [Required]
    public required string DatabaseUrl { get; set; }

    public string InventoryPublisherBindAddr { get; set; } = "tcp://127.0.0.1:5556";
}