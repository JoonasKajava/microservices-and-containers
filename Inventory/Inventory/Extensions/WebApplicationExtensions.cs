using Inventory.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Extensions;

public static class WebApplicationExtensions
{
    public static void MigrateDatabase(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
        dbContext.Database.Migrate();
    }
}