using Microsoft.EntityFrameworkCore;
using RenStore.Order.Domain.ReadModels;
using RenStore.Order.Persistence.EventStore;
using RenStore.Order.Persistence.Outbox;

namespace RenStore.Order.Persistence;

public class OrderingDbContext(
    DbContextOptions<OrderingDbContext> options) 
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderingDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<OrderReadModel> Orders { get; set; }
    public DbSet<OrderItemReadModel> OrderItem { get; set; }
    public DbSet<EventEntity> Events { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
}