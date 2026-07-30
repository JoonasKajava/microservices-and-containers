using Inventory.Context;
using Inventory.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Repositories;

public interface IInventoryRepository
{
    IEnumerable<Equipment> GetEquipments();
    Task<Equipment?> GetEquipmentByIdAsync(Guid id);
    Task DeleteEquipmentByIdAsync(Guid id);
    Task CreateEquipmentAsync(Equipment equipment);

    public Task<Equipment> UpdateEquipmentAsync(Guid id, Action<Equipment> updateAction);
}

public class InventoryRepository(InventoryDbContext dbContext) : IInventoryRepository
{
    public IEnumerable<Equipment> GetEquipments()
    {
        return dbContext.Equipment;
    }

    public async Task<Equipment?> GetEquipmentByIdAsync(Guid id)
    {
        return await dbContext.Equipment.FindAsync(id);
    }

    public async Task DeleteEquipmentByIdAsync(Guid id)
    {
        await dbContext.Equipment.Where(e => e.EquipmentId == id).ExecuteDeleteAsync();
    }

    public async Task CreateEquipmentAsync(Equipment equipment)
    {
        dbContext.Equipment.Add(equipment);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Equipment> UpdateEquipmentAsync(Guid id, Action<Equipment> updateAction)
    {
        var equipment = await dbContext.Equipment.FirstAsync(x => x.EquipmentId == id);

        updateAction(equipment);
        await dbContext.SaveChangesAsync();

        return equipment;
    }
}