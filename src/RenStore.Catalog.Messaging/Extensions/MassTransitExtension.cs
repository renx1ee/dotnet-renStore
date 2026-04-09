using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RenStore.Catalog.Messaging.Consumers;

namespace RenStore.Catalog.Messaging.Extensions;

public static class MassTransitExtension
{
    public static IServiceCollection AddCatalogMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<DiscountAvailabilityChangedConsumer>();
            
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(
                    host: configuration["RabbitMQ:Host"], 
                    virtualHost: configuration["RabbitMQ:VHost"], 
                    configure: h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"]!);
                        h.Password(configuration["RabbitMQ:Password"]!);
                    });
                
                /*cfg.ReceiveEndpoint("inventory.variant-size.created", e =>
                {
                    e.UseMessageRetry(r => r.Intervals(
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(15),
                        TimeSpan.FromSeconds(30)));
                    
                    e.ConfigureConsumer<DiscountAvailabilityChangedConsumer>(context);
                });*/
                
                /*cfg.ConfigureEndpoints(context);*/
            });
        });
        
        return services;
    }
}