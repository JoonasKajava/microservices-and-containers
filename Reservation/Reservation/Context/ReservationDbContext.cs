using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Reservation.Context;

public partial class ReservationDbContext : DbContext
{
    public DbSet<Entities.Reservation> Reservations { get; set; }

    private readonly IOptions<ReservationOptions> _reservationOptions;

    public ReservationDbContext(IOptions<ReservationOptions> reservationOptions)
    {
        _reservationOptions = reservationOptions;
    }

    public ReservationDbContext(DbContextOptions<ReservationDbContext> options,
        IOptions<ReservationOptions> reservationOptions)
        : base(options)
    {
        _reservationOptions = reservationOptions;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(_reservationOptions.Value.DatabaseUrl);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}