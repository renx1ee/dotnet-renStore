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
            x.AddConsumer<GetVariantSizePriceConsumer>();
            x.AddConsumer<DiscountAvailabilityChangedConsumer>();
            x.AddConsumer<ReviewsCountChangedConsumer>();
            x.AddConsumer<SellerIsVerifiedChangedConsumer>();
            x.AddConsumer<StockAvailabilityChangedConsumer>();
            
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
                
                cfg.ReceiveEndpoint("catalog.get-variant-size-price", e =>
                {
                    e.UseMessageRetry(r => r.Intervals(
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(15),
                        TimeSpan.FromSeconds(30)));
                
                    e.ConfigureConsumer<GetVariantSizePriceConsumer>(context);
                    /*e.Bind<DiscountAvailabilityChangedConsumer>();*/
                });
                
                cfg.ReceiveEndpoint("catalog.discount-availability-changed", e =>
                {
                    e.UseMessageRetry(r => r.Intervals(
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(15),
                        TimeSpan.FromSeconds(30)));
                
                    e.ConfigureConsumer<DiscountAvailabilityChangedConsumer>(context);
                    /*e.Bind<DiscountAvailabilityChangedConsumer>();*/
                });
            
                cfg.ReceiveEndpoint("reviews.reviews-count-changed", e =>
                {
                    e.UseMessageRetry(r => r.Intervals(
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(15),
                        TimeSpan.FromSeconds(30)));
                    e.ConfigureConsumer<ReviewsCountChangedConsumer>(context);
                    /*e.Bind<ReviewsCountChangedConsumer>();*/
                });
            
                cfg.ReceiveEndpoint("identity.seller-verified-changed", e =>
                {
                    e.UseMessageRetry(r => r.Intervals(
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(15),
                        TimeSpan.FromSeconds(30)));
                    e.ConfigureConsumer<SellerIsVerifiedChangedConsumer>(context);
                    /*e.Bind<SellerIsVerifiedChangedConsumer>();*/
                });
            
                cfg.ReceiveEndpoint("inventory.stock-availability-changed", e =>
                {
                    e.UseMessageRetry(r => r.Intervals(
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(15),
                        TimeSpan.FromSeconds(30)));
                    e.ConfigureConsumer<StockAvailabilityChangedConsumer>(context);
                    /*e.Bind<StockAvailabilityChangedConsumer>();*/
                });
                
                cfg.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
}