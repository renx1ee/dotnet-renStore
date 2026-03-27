using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RenStore.Inventory.Application.Abstractions;
using RenStore.Inventory.Persistence.EventStore;

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
        
        return services;
    }
}