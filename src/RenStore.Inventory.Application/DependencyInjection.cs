using Microsoft.Extensions.DependencyInjection;
using RenStore.Inventory.Application.Behaviors;
using RenStore.Inventory.Application.Common;

namespace RenStore.Inventory.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddInventoryApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly));
        
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(LoggingBehavior<,>));
        
        return services;
    }
}