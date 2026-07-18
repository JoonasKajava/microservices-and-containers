using Inventory.Context;
using Inventory.Contracts;
using Inventory.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class EquipmentController(
    ILogger<EquipmentController> logger,
    InventoryDbContext dbContext
) : ControllerBase
{
    [HttpGet(Name = "GetEquipment")]
    public IEnumerable<Equipment> Get()
    {
        logger.LogInformation("Getting equipment");
        return dbContext.Equipment;
    }

    [HttpPost(Name = "CreateEquipment")]
    public async Task<IActionResult> Post(CreateEquipment createEquipment)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var equipment = new Equipment
        {
            Name = createEquipment.Name,
            Description = createEquipment.Description
        };

        dbContext.Equipment.Add(equipment);
        await dbContext.SaveChangesAsync();


        return CreatedAtRoute("GetEquipment", equipment);
    }
}