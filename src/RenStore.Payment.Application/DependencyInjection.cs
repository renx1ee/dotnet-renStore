using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RenStore.Payment.Application.Abstractions.Services;
using RenStore.Payment.Application.Behaviors;
using RenStore.Payment.Application.Services;

namespace RenStore.Payment.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddPaymentApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly));

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(LoggingBehavior<,>));
        
        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ExceptionHandlingBehavior<,>));
        
        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidateBehavior<,>));

        services.AddScoped<IPaymentProviderService, YooKassaPaymentService>();
        
        return services;
    }
}