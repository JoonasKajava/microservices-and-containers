namespace Reservation.Contracts;

public class GetEquipmentResponse
{
    public Guid EquipmentId { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }
}