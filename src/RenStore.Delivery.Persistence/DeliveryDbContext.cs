using Microsoft.EntityFrameworkCore;
using RenStore.Delivery.Domain.Aggregates.Address;
using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.Delivery.Persistence.EntityTypeConfigurations;
using RenStore.Delivery.Persistence.EventStore;
using RenStore.Delivery.Persistence.Outbox;

namespace RenStore.Delivery.Persistence;

public sealed class DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : DbContext(options)
{
    public DbSet<EventEntity>               Events            => Set<EventEntity>();
    public DbSet<OutboxMessage>             OutboxMessages    => Set<OutboxMessage>();
    public DbSet<DeliveryOrderReadModel>    DeliveryOrders    => Set<DeliveryOrderReadModel>();
    public DbSet<DeliveryTrackingReadModel> DeliveryTrackings => Set<DeliveryTrackingReadModel>();
    public DbSet<Country>                   Countries         => Set<Country>();
    public DbSet<City>                      Cities            => Set<City>();
    public DbSet<Address>                   Addresses         => Set<Address>();
    public DbSet<DeliveryTariff>            DeliveryTariffs   => Set<DeliveryTariff>();
    
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