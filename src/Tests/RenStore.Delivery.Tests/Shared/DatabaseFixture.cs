using Microsoft.EntityFrameworkCore;
using RenStore.Delivery.Persistence;

namespace RenStore.Delivery.Tests.Shared;

public class DatabaseFixture : IDisposable, IAsyncDisposable
{
    private readonly DeliveryDbContext _context;
    
    public static readonly string ConnectionString = "Server=localhost;Port=5432;DataBase=UnitRenstoreTests; User Id=re;Password=postgres;Include Error Detail=True";
    
    public DatabaseFixture(DeliveryDbContext context)
    {
        _context = CreateReadyContext();
    }
    
    public static DeliveryDbContext CreateReadyContext()
    {
        var options = new DbContextOptionsBuilder<DeliveryDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        var context = new DeliveryDbContext(options);
        
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        /*SeedData(context);*/
        
        context.SaveChanges();
        context.ChangeTracker.Clear();
        
        return context;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}