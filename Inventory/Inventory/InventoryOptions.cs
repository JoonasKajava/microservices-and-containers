using System.ComponentModel.DataAnnotations;

namespace Inventory;

public class InventoryOptions
{
   public static string SectionName => "Inventory";

   [Required]
   public required string DatabaseUrl { get; set; }

   [Required]
   public required string ReservationStatePubAddr { get; set; }

   public string ReservationStatePubTopic { get; set; } = "reservation";
}