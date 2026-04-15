using Microsoft.Extensions.DependencyInjection;

namespace RenStore.Order.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddOrderingApplication(
        this IServiceCollection services)
    {
        return services;
    }
}