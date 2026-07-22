using Microsoft.EntityFrameworkCore;
using Reservation.Context;

namespace Reservation.Repositories;

public interface IReservationsRepository
{
    public IEnumerable<Entities.Reservation> GetReservationsForEquipment(Guid equipmentId);
    public Task DeleteReservationAsync(Guid id);
    public Task DeleteReservationsForEquipmentAsync(Guid id);
    public Task CreateReservationAsync(Entities.Reservation reservation);

    public IEnumerable<Entities.Reservation> GetOverlappingReservationsForEquipment(Guid equipmentId,
        DateTimeOffset start, DateTimeOffset end);
}

public class ReservationsRepository(ReservationDbContext dbContext) : IReservationsRepository
{
    public IEnumerable<Entities.Reservation> GetReservationsForEquipment(Guid equipmentId)
    {
        return dbContext.Reservations.Where(x => x.EquipmentId == equipmentId);
    }

    public async Task DeleteReservationAsync(Guid id)
    {
        await dbContext.Reservations.Where(e => e.ReservationId == id).ExecuteDeleteAsync();
    }

    public async Task DeleteReservationsForEquipmentAsync(Guid id)
    {
        await dbContext.Reservations.Where(e => e.EquipmentId == id).ExecuteDeleteAsync();
    }

    public async Task CreateReservationAsync(Entities.Reservation reservation)
    {
        dbContext.Reservations.Add(reservation);

        await dbContext.SaveChangesAsync();
    }

    public IEnumerable<Entities.Reservation> GetOverlappingReservationsForEquipment(Guid equipmentId, DateTimeOffset start, DateTimeOffset end)
    {
        return dbContext.Reservations.Where(x =>
            x.EquipmentId == equipmentId && x.StartTime < end && x.EndTime > start).ToList();
    }
}