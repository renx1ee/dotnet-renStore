using Microsoft.EntityFrameworkCore;
using RenStore.Inventory.Persistence;

namespace RenStore.Inventory.Tests.Persistence.Integration.Repository;

public sealed class InventoryRepositoryTestsBase
{
    public readonly static string ConnectionString =
        $"Server=localhost;Port=5432;DataBase={Guid.NewGuid()}; User Id=re;Password=postgres;Include Error Detail=True";
    
    public static string BuildConnectionString(Guid id)
    {
        if (id == Guid.Empty)
            throw new InvalidOperationException(nameof(id));
        
        return $"Server=localhost;Port=5432;DataBase={id}; User Id=re;Password=postgres;Include Error Detail=True";
    }
    
    public static async Task<InventoryDbContext> CreateContext()
    {
        var options = new DbContextOptionsBuilder<InventoryDbContext>()
            .UseNpgsql(connectionString: ConnectionString)
            .Options;
        
        var context = new InventoryDbContext(options);
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        
        return context;
    }
}