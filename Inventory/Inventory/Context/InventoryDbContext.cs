using Inventory.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Inventory.Context;

public partial class InventoryDbContext : DbContext
{
    private InventoryOptions _inventoryOptions;
    public DbSet<Equipment> Equipment { get; set; }

    public InventoryDbContext(InventoryOptions inventoryOptions)
    {
        _inventoryOptions = inventoryOptions;
    }

    public InventoryDbContext(DbContextOptions<InventoryDbContext> options, IOptions<InventoryOptions> inventoryOptions)
        : base(options)
    {
        _inventoryOptions = inventoryOptions.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(_inventoryOptions.DatabaseUrl);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}