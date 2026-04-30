using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace RenStore.Order.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddOrderingApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly));

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        return services;
    }
}