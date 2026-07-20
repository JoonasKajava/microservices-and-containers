using Microsoft.EntityFrameworkCore;
using Reservation.Context;

namespace Reservation.Extensions;

public static class WebApplicationExtensions
{
    public static void MigrateDatabase(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ReservationDbContext>();
        dbContext.Database.Migrate();
    }
}