namespace Inventory;

public class InventoryOptions
{
    public static string SectionName => "Inventory";

    public string InventoryPublisherBindAddr { get; set; } = "tcp://0.0.0.0:5556";

}