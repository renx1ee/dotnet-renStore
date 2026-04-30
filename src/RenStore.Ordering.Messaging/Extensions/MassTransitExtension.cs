using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RenStore.Order.Application.Saga;
using RenStore.Order.Persistence;
using RenStore.Ordering.Messaging.Consumers;

namespace RenStore.Ordering.Messaging.Extensions;

public static class MassTransitExtension
{
    public static IServiceCollection AddInventoryMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddSagaStateMachine<PlaceOrderSaga, PlaceOrderSagaState>()
                .EntityFrameworkRepository(r =>
                {
                    r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                    r.AddDbContext<DbContext, OrderingDbContext>((_, b) =>
                    {
                        var connectionString = configuration.GetConnectionString("SagaConnection");
                        b.UseNpgsql(connectionString);
                    });
                });
            
            x.AddConsumer<CreateOrderConsumer>();
            
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
                
                cfg.ReceiveEndpoint("inventory.variant-size-created", e =>
                {
                    e.UseMessageRetry(r => r.Intervals(
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(15),
                        TimeSpan.FromSeconds(30)));
                    
                    e.ConfigureConsumer<CreateOrderConsumer>(context);
                });
                
                cfg.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
}