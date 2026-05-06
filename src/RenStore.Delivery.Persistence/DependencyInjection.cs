using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RenStore.Catalog.Persistence.Outbox;
using RenStore.Delivery.Application.Abstractions;
using RenStore.Delivery.Application.Abstractions.Projections;
using RenStore.Delivery.Application.Abstractions.Queries;
using RenStore.Delivery.Domain.Interfaces;
using RenStore.Delivery.Persistence.EventStore;
using RenStore.Delivery.Persistence.Outbox;
using RenStore.Delivery.Persistence.Read.Queries.Postgresql;
using RenStore.Delivery.Persistence.Write.Projections;
using RenStore.Delivery.Persistence.Write.Repositories;

namespace RenStore.Delivery.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddDeliveryPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<DeliveryDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString);
        });
        
        services.AddHostedService<OutboxWorker>();
        
        // dotnet add package Microsoft.Extensions.Options.ConfigurationExtensions
        services.Configure<OutboxOptions>(
            configuration.GetSection(OutboxOptions.SectionName));
        
        services.AddScoped<IIntegrationOutboxWriter, IntegrationOutboxWriter>();
        
        services.AddScoped<IEventStore, SqlEventStore>();
        services.AddScoped<IEventStore, SqlEventStore>();

        services.AddScoped<IAddressQuery, AddressQuery>();
        services.AddScoped<IDeliveryTariffQuery, DeliveryTariffQuery>();
        services.AddScoped<ICountryQuery, CountryQuery>();
        services.AddScoped<ICityQuery, CityQuery>();
        services.AddScoped<IDeliveryOrderQuery, DeliveryOrderQuery>();
        services.AddScoped<IDeliveryTrackingQuery, DeliveryTrackingQuery>();
        
        services.AddScoped<IDeliveryTrackingProjection, DeliveryTrackingProjection>();
        services.AddScoped<IDeliveryOrderProjection, DeliveryOrderProjection>();
        
        // Entities
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IDeliveryTariffRepository, DeliveryTariffRepository>();
        // Aggregates
        services.AddScoped<IDeliveryOrderRepository, DeliveryOrderRepository>();
        
        return services;
    }
}