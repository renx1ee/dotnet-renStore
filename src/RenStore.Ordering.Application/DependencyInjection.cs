using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RenStore.Order.Application.Behaviors;

namespace RenStore.Order.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddOrderingApplication(
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