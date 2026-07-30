using Inventory.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Context;

public partial class InventoryDbContext : DbContext
{
    public DbSet<Equipment> Equipment { get; set; }

    public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}