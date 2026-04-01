using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RenStore.Inventory.Messaging.Consumers;

namespace RenStore.Inventory.Messaging.Extensions;

public static class MassTransitExtension
{
    public static IServiceCollection AddInventoryMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<VariantSizeCreatedConsumer>();
            
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
                
                cfg.ReceiveEndpoint("inventory.variant-size.created", e =>
                {
                    e.UseMessageRetry(r => r.Intervals(
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(15),
                        TimeSpan.FromSeconds(30)));
                    
                    e.ConfigureConsumer<VariantSizeCreatedConsumer>(context);
                });
                
                /*cfg.ConfigureEndpoints(context);*/
            });
        });
        
        return services;
    }
}

/*x.AddConsumer<VariantDeletedConsumer>();*/
/*x.AddConsumer<OrderCreatedConsumer>();*/
/*x.AddConsumer<OrderDeletedConsumer>();*/
/*x.AddConsumer<OrderCancelledConsumer>();*/

/*cfg.ReceiveEndpoint("inventory.variant-size.deleted", e =>
                {
                    e.ConfigureConsumer<VariantSizeDeletedConsumer>(context);
                });

                cfg.ReceiveEndpoint("inventory.order.created", e =>
                {
                    e.ConfigureConsumer<VariantCreatedConsumer>(context);
                });*/