namespace Inventory;

public class InventoryOptions
{
    public static string SectionName => "Inventory";

    public string InventoryPublisherBindAddr { get; set; } = "tcp://127.0.0.1:5556";

}