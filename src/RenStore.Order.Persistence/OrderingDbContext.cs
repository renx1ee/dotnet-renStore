using Microsoft.EntityFrameworkCore;
using RenStore.Order.Domain.Aggregates.Order;
using RenStore.Order.Persistence.EventStore;

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
    
    public DbSet<Domain.Aggregates.Order.Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItem { get; set; }
    public DbSet<EventEntity> Events { get; set; }
}