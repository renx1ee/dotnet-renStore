using Microsoft.EntityFrameworkCore;
using RenStore.Catalog.Persistence;

namespace RenStore.Catalog.Tests.UnitTets.Persistence.Repositories;

public class CatalogRepositoryTestsBase
{
    public readonly static string ConnectionString =
        $"Server=localhost;Port=5432;DataBase={Guid.NewGuid()}; User Id=re;Password=postgres;Include Error Detail=True";
    
    public static string BuildConnectionString(Guid id)
    {
        if (id == Guid.Empty)
            throw new InvalidOperationException(nameof(id));
        
        return $"Server=localhost;Port=5432;DataBase={id}; User Id=re;Password=postgres;Include Error Detail=True";
    }
    
    public static async Task<CatalogDbContext> CreateContext()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseNpgsql(connectionString: ConnectionString)
            .Options;
        
        var context = new CatalogDbContext(options);
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        
        return context;
    }
}