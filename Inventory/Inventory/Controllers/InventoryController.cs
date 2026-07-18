using Microsoft.AspNetCore.Mvc;

namespace Inventory.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class InventoryController(ILogger<InventoryController> logger) : ControllerBase
{

    [HttpGet(Name = "GetInventory")]
    public string Get()
    {
        logger.LogInformation("Getting inventory");
        return "asd jotain";
    }
}