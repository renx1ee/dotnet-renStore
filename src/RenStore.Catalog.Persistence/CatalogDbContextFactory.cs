using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace RenStore.Catalog.Persistence;

public class CatalogDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
{
    public CatalogDbContext CreateDbContext(string[] args)
    {
        // macOS/linux: export CONNECTION_STRING="Server=localhost;Port=5432;DataBase=renstore_catalog; User Id=re;Password=postgres ;Include Error Detail=True"
        var connectionString =
            Environment.GetEnvironmentVariable("CONNECTION_STRING")
            ?? "Server=localhost;Port=5432;DataBase=renstore_catalog; User Id=re;Password=postgres ;Include Error Detail=True";
        
        var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
        
        optionsBuilder.UseNpgsql(connectionString);

        return new CatalogDbContext(optionsBuilder.Options);
    }
}