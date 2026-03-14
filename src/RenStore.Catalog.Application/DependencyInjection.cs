using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RenStore.Catalog.Application.Behaviors;
using RenStore.Catalog.Application.Service;
using RenStore.Catalog.Domain.DomainService;

namespace RenStore.Catalog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IPublishProductService, PublishProductService>();
        services.AddScoped<IStorageService, LocalStorageService>();
        
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