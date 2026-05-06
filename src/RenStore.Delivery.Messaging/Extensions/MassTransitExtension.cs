using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RenStore.Delivery.Messaging.Consumers;

namespace RenStore.Delivery.Messaging.Extensions;

public static class MassTransitExtension
{
    public static IServiceCollection AddDeliveryMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderPlacementCompletedConsumer>();
            x.AddConsumer<OrderCancelledConsumer>();
            /*x.AddConsumer<PublishDeliveryStatusChangedConsumer>();*/
            
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
                
                cfg.ReceiveEndpoint("delivery-order-placement-completed", e =>
                {
                    e.UseMessageRetry(r => r.Intervals(
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(15),
                        TimeSpan.FromSeconds(30)));
                    
                    e.ConfigureConsumer<OrderPlacementCompletedConsumer>(context);
                });
                
                cfg.ReceiveEndpoint("delivery-order-cancelled", e =>
                {
                    e.UseMessageRetry(r => r.Intervals(
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(15),
                        TimeSpan.FromSeconds(30)));
                    
                    e.ConfigureConsumer<OrderCancelledConsumer>(context);
                });
                
                cfg.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
}