using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RenStore.Delivery.Persistence;

public class DeliveryDbContextFactory : IDesignTimeDbContextFactory<DeliveryDbContext>
{
    public DeliveryDbContext CreateDbContext(string[] args)
    {
        // macOS/linux: export CONNECTION_STRING="Server=localhost;Port=5432;DataBase=renstore_delivery; User Id=re;Password=postgres ;Include Error Detail=True"
        var connectionString =
            Environment.GetEnvironmentVariable("CONNECTION_STRING")
            ?? "Server=localhost;Port=5432;DataBase=renstore_delivery; User Id=re;Password=postgres ;Include Error Detail=True";
        
        var optionsBuilder = new DbContextOptionsBuilder<DeliveryDbContext>();
        
        optionsBuilder.UseNpgsql(connectionString);

        return new DeliveryDbContext(optionsBuilder.Options);
    }
}