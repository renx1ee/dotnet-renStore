using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RenStore.Order.Application.Abstractions;
using RenStore.Order.Persistence.EventStore;

namespace RenStore.Order.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddOrderingPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<OrderingDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        
        services.AddScoped<IEventStore, SqlEventStore>();
        
        return services;
    }
}