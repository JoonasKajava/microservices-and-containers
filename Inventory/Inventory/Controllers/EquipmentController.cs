using Inventory.Context;
using Inventory.Contracts;
using Inventory.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Inventory.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class EquipmentController(
    ILogger<EquipmentController> logger,
    InventoryDbContext dbContext,
    IOptions<InventoryOptions> options
) : ControllerBase
{
    [HttpGet(Name = "GetEquipment")]
    public IEnumerable<Equipment> Get()
    {
        logger.LogInformation("Getting equipment");
        return dbContext.Equipment;
    }

    [HttpGet("{id}", Name = "GetEquipmentById")]
    public async Task<ActionResult<Equipment>> Get(Guid id)
    {
        var equipment = await dbContext.Equipment.FindAsync(id);

        if (equipment is null) return NotFound();

        return equipment;
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

    [HttpDelete("{id}", Name = "DeleteEquipment")]
    public async Task<IActionResult> Delete(Guid id)
    {
        // TODO: Check reservations?
        await dbContext.Equipment.Where(e => e.EquipmentId == id).ExecuteDeleteAsync();
        return NoContent();
    }
}