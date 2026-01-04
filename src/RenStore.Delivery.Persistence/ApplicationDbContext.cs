using Microsoft.EntityFrameworkCore;
using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Persistence.EntityTypeConfigurations;

namespace RenStore.Delivery.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AddressConfiguration());
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<DeliveryOrder> DeliveryOrders { get; set; }
    public DbSet<DeliveryTariff> DeliveryTariffs { get; set; }
    public DbSet<DeliveryTracking> DeliveryTrackings { get; set; }
    public DbSet<PickUpPoint> PickUpPoints { get; set; }
    public DbSet<SortingCenter> SortingCenters { get; set; }
}