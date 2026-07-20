using System.ComponentModel.DataAnnotations;

namespace Inventory;

public class InventoryOptions
{
   public static string SectionName => "Inventory";

   [Required]
   public required string DatabaseUrl { get; set; }
}