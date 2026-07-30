using System.ComponentModel.DataAnnotations;

namespace Inventory.Contracts;

public class CreateEquipment
{
    [Required]
    [MaxLength(255)]
    public required string Name { get; set; }

    [MaxLength(1024)]
    public string? Description { get; set; }
}