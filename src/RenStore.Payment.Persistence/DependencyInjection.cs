using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RenStore.Payment.Application.Abstractions;
using RenStore.Payment.Application.Abstractions.Projection;
using RenStore.Payment.Application.Abstractions.Queries;
using RenStore.Payment.Domain.Interfaces;
using RenStore.Payment.Persistence.EventStore;
using RenStore.Payment.Persistence.Outbox;
using RenStore.Payment.Persistence.Read.Queries.Postgresql;
using RenStore.Payment.Persistence.Write.Projections;
using RenStore.Payment.Persistence.Write.Repositories;

namespace RenStore.Payment.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPaymentPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<PaymentDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString);
        });

        services.AddHostedService<OutboxWorker>();
        
        services.Configure<OutboxOptions>(
            configuration.GetSection(OutboxOptions.SectionName));
        
        services.AddScoped<IIntegrationOutboxWriter, IntegrationOutboxWriter>();

        services.AddScoped<IEventStore, SqlEventStore>();
        
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        
        services.AddScoped<IPaymentAttemptQuery, PaymentAttemptQuery>();
        services.AddScoped<IPaymentQuery, PaymentQuery>();
        services.AddScoped<IRefundQuery, RefundQuery>();
        
        services.AddScoped<IPaymentAttemptProjection, PaymentAttemptProjection>();
        services.AddScoped<IPaymentProjection, PaymentProjection>();
        services.AddScoped<IRefundProjection, RefundProjection>();
        
        return services;
    }
}