using Microsoft.EntityFrameworkCore;
using RenStore.Payment.Domain.ReadModels;
using RenStore.Payment.Persistence.EventStore;
using RenStore.Payment.Persistence.Outbox;

namespace RenStore.Payment.Persistence;

public sealed class PaymentDbContext(DbContextOptions<PaymentDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<OutboxMessage>           OutboxMessages { get; set; }
    public DbSet<EventEntity>             Events         { get; set; }
    public DbSet<PaymentAttemptReadModel> PaymentAttempts       { get; set; }
    public DbSet<PaymentReadModel>        Payments       { get; set; }
    public DbSet<RefundReadModel>         Refunds        { get; set; }
}