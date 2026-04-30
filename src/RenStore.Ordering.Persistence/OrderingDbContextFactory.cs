using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RenStore.Order.Persistence;

public class OrderingDbContextFactory : IDesignTimeDbContextFactory<OrderingDbContext>
{
    public OrderingDbContext CreateDbContext(string[] args)
    {
        // macOS/linux: export CONNECTION_STRING="Server=localhost;Port=5432;DataBase=renstore_catalog; User Id=re;Password=postgres ;Include Error Detail=True"
        var connectionString =
            Environment.GetEnvironmentVariable("CONNECTION_STRING")
            ?? "Server=localhost;Port=5432;DataBase=renstore_ordering; User Id=re;Password=postgres ;Include Error Detail=True";
        
        var optionsBuilder = new DbContextOptionsBuilder<OrderingDbContext>();
        
        optionsBuilder.UseNpgsql(connectionString);

        return new OrderingDbContext(optionsBuilder.Options);
    }
}