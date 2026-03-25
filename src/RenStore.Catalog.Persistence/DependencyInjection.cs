using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Application.Abstractions.Projections;
using RenStore.Catalog.Application.Abstractions.Queries;
using RenStore.Catalog.Domain.Interfaces.Repository;
using RenStore.Catalog.Persistence.EventStore;
using RenStore.Catalog.Persistence.Read.Queries.Postgresql;
using RenStore.Catalog.Persistence.Write.Projections;
using RenStore.Catalog.Persistence.Write.Repositories.Postgresql;

namespace RenStore.Catalog.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<CatalogDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        /*services.AddMassTransit(x =>
        {
            /*x.AddConsumer<>();#1#
            
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(
                    host: configuration["RabbitMQ:Host"], 
                    virtualHost: configuration["RabbitMQ:VHost"], 
                    configure: h =>
                {
                    h.Username(configuration["RabbitMQ:Username"]);
                    h.Password(configuration["RabbitMQ:Password"]);
                });
                
                cfg.ConfigureEndpoints(context);
            });
        });*/
        
        services.AddScoped<IEventStore, SqlEventStore>();
        
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
        services.AddScoped<IVariantImageRepository, VariantImageRepository>();
        
        services.AddScoped<ICategoryProjection, CategoryProjection>();
        services.AddScoped<ISubCategoryProjection, SubCategoryProjection>();
        services.AddScoped<IProductProjection, ProductProjection>();
        services.AddScoped<ISizePriceProjection, SizePriceProjection>();
        services.AddScoped<IProductVariantProjection, ProductVariantProjection>();
        services.AddScoped<IProductVariantSizeProjection, ProductVariantSizeProjection>();
        services.AddScoped<IVariantAttributeProjection, VariantAttributeProjection>();
        services.AddScoped<IVariantDetailProjection, VariantDetailProjection>();
        services.AddScoped<IVariantImageProjection, VariantImageProjection>();
        
        services.AddScoped<IVariantSizeQuery, VariantSizeQuery>();
        services.AddScoped<IVariantAttributeQuery, VariantAttributeQuery>();
        services.AddScoped<IVariantImageQuery, VariantImageQuery>();
        services.AddScoped<IVariantDetailQuery, VariantDetailQuery>();
        services.AddScoped<IProductVariantQuery, ProductVariantQuery>();
        services.AddScoped<IProductQuery, ProductQuery>();
        services.AddScoped<IPriceHistoryQuery, PriceHistoryQuery>();
        
        return services;
    }   
}