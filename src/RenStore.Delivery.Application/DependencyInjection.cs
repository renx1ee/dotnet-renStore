using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RenStore.Delivery.Application.Behabiors;

namespace RenStore.Delivery.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddDeliveryApplication(
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
        
        return services;
    }
}