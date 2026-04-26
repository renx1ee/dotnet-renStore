using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RenStore.Order.Application.Abstractions;
using RenStore.Order.Application.Abstractions.Projections;
using RenStore.Order.Domain.Interfaces;
using RenStore.Order.Persistence.EventStore;
using RenStore.Order.Persistence.Outbox;
using RenStore.Order.Persistence.Write.Projections;
using RenStore.Order.Persistence.Write.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RenStore.Order.Application.Abstractions.Queries;
using RenStore.Order.Persistence.Read.Queries;

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
        
        services.Configure<OutboxOptions>(
            configuration.GetSection(OutboxOptions.SectionName));
        
        services.AddScoped<IIntegrationOutboxWriter, IntegrationOutboxWriter>();
        
        services.AddHostedService<OutboxWorker>();
        
        services.AddScoped<IEventStore, SqlEventStore>();
        
        services.AddScoped<IOrderRepository, OrderRepository>();
        
        services.AddScoped<IOrderProjection, OrderProjection>();
        services.AddScoped<IOrderItemProjection, OrderItemProjection>();
        
        services.AddScoped<IOrderItemQuery, OrderItemQuery>();
        services.AddScoped<IOrderQuery, OrderQuery>();
        
        return services;
    }
}