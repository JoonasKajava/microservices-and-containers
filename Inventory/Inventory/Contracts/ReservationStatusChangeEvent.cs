namespace Inventory.Contracts;

public class ReservationStatusChangeEvent
{
    public Guid ReservationId { get; set; }
    public Guid EquipmentId { get; set; }
}