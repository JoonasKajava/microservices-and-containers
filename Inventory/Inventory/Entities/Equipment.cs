using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Entities;

public class Equipment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid EquipmentId { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Name { get; set; }

    [MaxLength(1024)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Creator { get; set; }

    [Required]
    public required EquipmentAvailability Availability { get; set; }

}

public enum EquipmentAvailability
{
    Available = 0,
    InUse = 1
}