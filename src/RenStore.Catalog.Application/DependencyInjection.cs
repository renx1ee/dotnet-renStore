using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RenStore.Catalog.Application.Behaviors;
using RenStore.Catalog.Application.Features.Product.Commands.Create;

namespace RenStore.Catalog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogApplication(
        IServiceCollection services)
    {
        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(LoggingBehavior<,>));
        
        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ExceptionHandlingBehavior<,>));

        services.AddTransient(
            typeof(ValidateBehavior<,>),
            typeof(ExceptionHandlingBehavior<,>));

        services.AddMediatR(x =>
            x.RegisterServicesFromAssemblies(
                typeof(CreateProductCommand).Assembly,
                typeof(CreateProductCommandHandler).Assembly));

        services.AddValidatorsFromAssembly(
            typeof(CreateProductCommandValidator).Assembly);
        
        return services;
    }
}