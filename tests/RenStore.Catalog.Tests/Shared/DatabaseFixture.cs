using Microsoft.EntityFrameworkCore;
using RenStore.Catalog.Persistence;

namespace RenStore.Catalog.Tests.Shared;

public class DatabaseFixture : IDisposable
{
    public static readonly string ConnectionString = "Server=localhost;Port=5432;DataBase=UnitRenstoreTests; User Id=re;Password=postgres;Include Error Detail=True";
    
    private readonly CatalogDbContext _context;
    
    public DatabaseFixture(CatalogDbContext context)
    {
        _context = CreateReadyContext();
    }

    public static CatalogDbContext CreateReadyContext()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseNpgsql(connectionString: ConnectionString)
            .Options;

        var context = new CatalogDbContext(options);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.SaveChanges();
        context.ChangeTracker.Clear();

        return context;
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
}