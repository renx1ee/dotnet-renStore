using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RenStore.Inventory.Application.Abstractions;
using RenStore.Inventory.Application.Abstractions.Projections;
using RenStore.Inventory.Application.Abstractions.ReadRepository;
using RenStore.Inventory.Domain.Interfaces.Repository;
using RenStore.Inventory.Persistence.EventStore;
using RenStore.Inventory.Persistence.Outbox;
using RenStore.Inventory.Persistence.Read.Repository;
using RenStore.Inventory.Persistence.Write.Projections;
using RenStore.Inventory.Persistence.Write.Repositories;

namespace RenStore.Inventory.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInventoryPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<InventoryDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddHostedService<OutboxWorker>();
        
        services.Configure<OutboxOptions>(
            configuration.GetSection(OutboxOptions.SectionName));

        services.AddScoped<IEventStore, SqlEventStore>();
        
        services.AddScoped<IStockRepository, StockRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        
        services.AddScoped<IStockReadRepository, StockReadRepository>();
        
        services.AddScoped<IReservationProjection, ReservationProjection>();
        services.AddScoped<IStockProjection, StockProjection>();
        
        return services;
    }
}