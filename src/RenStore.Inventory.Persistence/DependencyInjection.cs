using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RenStore.Inventory.Application.Abstractions;
using RenStore.Inventory.Application.Abstractions.Projections;
using RenStore.Inventory.Persistence.EventStore;
using RenStore.Inventory.Persistence.Write.Projections;

namespace RenStore.Inventory.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInventoryPersistence(
        this ServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<InventoryDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IEventStore, SqlEventStore>();
        
        services.AddScoped<IReservationProjection, ReservationProjection>();
        services.AddScoped<IStockProjection, StockProjection>();
        
        return services;
    }
}