using Microsoft.EntityFrameworkCore;
using Reservation.Context;

namespace Reservation.Repositories;

public interface IReservationsRepository
{
    public Task<Entities.Reservation?> GetReservation(Guid id);
    public IEnumerable<Entities.Reservation> GetReservationsForEquipment(Guid equipmentId);
    public Task DeleteReservationAsync(Guid id);
    public Task DeleteReservationsForEquipmentAsync(Guid id);
    public Task CreateReservationAsync(Entities.Reservation reservation);
    public Task<Entities.Reservation> UpdateReservationAsync(Guid id, Action<Entities.Reservation> updateAction);

    public IEnumerable<Entities.Reservation> GetOverlappingReservationsForEquipment(Guid equipmentId,
        DateTimeOffset start, DateTimeOffset end);
}

public class ReservationsRepository(ReservationDbContext dbContext) : IReservationsRepository
{
    public async Task<Entities.Reservation?> GetReservation(Guid id)
    {
        return await dbContext.Reservations.FirstOrDefaultAsync(x => x.ReservationId == id);
    }

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

    public async Task<Entities.Reservation> UpdateReservationAsync(Guid id, Action<Entities.Reservation> updateAction)
    {
        var reservation = await dbContext.Reservations.FirstAsync(x => x.ReservationId == id);

        updateAction(reservation);
        await dbContext.SaveChangesAsync();

        return reservation;
    }


    public IEnumerable<Entities.Reservation> GetOverlappingReservationsForEquipment(Guid equipmentId,
        DateTimeOffset start, DateTimeOffset end)
    {
        return dbContext.Reservations.Where(x =>
            x.EquipmentId == equipmentId && x.StartTime < end && x.EndTime > start).ToList();
    }
}