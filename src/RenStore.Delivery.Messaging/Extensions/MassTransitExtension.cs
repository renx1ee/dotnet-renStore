using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RenStore.Delivery.Messaging.Extensions;

public static class MassTransitExtension
{
    public static IServiceCollection AddDeliveryMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
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
                
                cfg.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
}