using Microsoft.EntityFrameworkCore;
using RenStore.Inventory.Domain.ReadModels;
using RenStore.Inventory.Persistence.EntityTypeConfigurations;
using RenStore.Inventory.Persistence.EventStore;
using RenStore.Inventory.Persistence.Outbox;

namespace RenStore.Inventory.Persistence;

public sealed class InventoryDbContext(
    DbContextOptions<InventoryDbContext> options) 
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EventEntityConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxConfiguration());
        modelBuilder.ApplyConfiguration(new VariantReservationReadModelConfiguration());
        modelBuilder.ApplyConfiguration(new VariantStockReadModelConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<EventEntity> Events { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<VariantReservationReadModel> Reservations { get; set; }
    public DbSet<VariantStockReadModel> Stocks { get; set; }
}