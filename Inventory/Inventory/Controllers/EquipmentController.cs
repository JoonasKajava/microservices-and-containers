using Inventory.Contracts;
using Inventory.Entities;
using Inventory.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Controllers;

[ApiController]
[Authorize]
[Route("/api/v1/[controller]")]
public class EquipmentController(
    ILogger<EquipmentController> logger,
    IInventoryRepository inventoryRepository,
    IInventoryPublisher inventoryPublisher
) : ControllerBase
{
    [HttpGet(Name = "GetEquipment")]
    public IEnumerable<Equipment> Get()
    {
        logger.LogInformation("Getting equipment list");
        return inventoryRepository.GetEquipments();
    }

    [HttpGet("{id}", Name = "GetEquipmentById")]
    public async Task<ActionResult<Equipment>> Get(Guid id)
    {
        var equipment = await inventoryRepository.GetEquipmentByIdAsync(id);

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
            Creator = HttpContext.User.Identity!.Name,
            Name = createEquipment.Name,
            Description = createEquipment.Description
        };

        await inventoryRepository.CreateEquipmentAsync(equipment);

        return CreatedAtRoute("GetEquipment", equipment);
    }

    [HttpDelete("{id}", Name = "DeleteEquipment")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await inventoryRepository.DeleteEquipmentByIdAsync(id);
        await inventoryPublisher.EquipmentDeleted(id);
        return NoContent();
    }
}