using Microsoft.EntityFrameworkCore;
using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Persistence.EntityTypeConfigurations;

namespace RenStore.Delivery.Persistence;

public sealed class DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AddressConfiguration());
        modelBuilder.ApplyConfiguration(new CityConfiguration());
        modelBuilder.ApplyConfiguration(new CountryConfiguration());
        modelBuilder.ApplyConfiguration(new DeliveryOrderConfiguration());
        modelBuilder.ApplyConfiguration(new DeliveryTariffConfiguration());
        modelBuilder.ApplyConfiguration(new DeliveryTrackingConfiguration());
        modelBuilder.ApplyConfiguration(new PickupPointConfiguration());
        modelBuilder.ApplyConfiguration(new SortingCenterConfiguration());
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<DeliveryOrder> DeliveryOrders { get; set; }
    public DbSet<DeliveryTariff> DeliveryTariffs { get; set; }
    public DbSet<DeliveryTracking> DeliveryTrackings { get; set; }
    public DbSet<PickupPoint> PickupPoints { get; set; }
    public DbSet<SortingCenter> SortingCenters { get; set; }
}