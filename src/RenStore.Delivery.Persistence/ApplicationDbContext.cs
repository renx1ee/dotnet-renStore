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
}