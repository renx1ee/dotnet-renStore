using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RenStore.Payment.Messaging.Extensions;

public static class MassTransitExtension
{
    public static IServiceCollection AddPaymentMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            
            /*x.AddConsumer<CreateOrderConsumer>();*/
            
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
                
                /*cfg.ReceiveEndpoint("inventory.variant-size-created", e =>
                {
                    e.UseMessageRetry(r => r.Intervals(
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(15),
                        TimeSpan.FromSeconds(30)));
                    
                    e.ConfigureConsumer<CreateOrderConsumer>(context);
                });*/
                
                cfg.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
}