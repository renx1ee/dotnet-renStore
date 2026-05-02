using Microsoft.EntityFrameworkCore;
using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.Delivery.Persistence.EntityTypeConfigurations;
using RenStore.Delivery.Persistence.EventStore;
using RenStore.Delivery.Persistence.Outbox;

namespace RenStore.Delivery.Persistence;

public sealed class DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : DbContext(options)
{
    public DbSet<DeliveryOrderReadModel>    DeliveryOrders    => Set<DeliveryOrderReadModel>();
    public DbSet<DeliveryTrackingReadModel> DeliveryTrackings => Set<DeliveryTrackingReadModel>();
    public DbSet<CountryReadModel>          Countries         => Set<CountryReadModel>();
    public DbSet<CityReadModel>             Cities            => Set<CityReadModel>();
    public DbSet<AddressReadModel>          Addresses         => Set<AddressReadModel>();
    public DbSet<DeliveryTariffReadModel>   DeliveryTariffs   => Set<DeliveryTariffReadModel>();
    public DbSet<EventEntity>               Events            => Set<EventEntity>();
    public DbSet<OutboxMessage>             OutboxMessages    => Set<OutboxMessage>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DeliveryOrderConfiguration());
        modelBuilder.ApplyConfiguration(new DeliveryTrackingConfiguration());
        modelBuilder.ApplyConfiguration(new CountryConfiguration());
        modelBuilder.ApplyConfiguration(new CityConfiguration());
        modelBuilder.ApplyConfiguration(new AddressConfiguration());
        modelBuilder.ApplyConfiguration(new DeliveryTariffConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}