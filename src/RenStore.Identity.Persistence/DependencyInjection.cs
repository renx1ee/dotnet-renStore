using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RenStore.Identity.Application.Abstractions;
using RenStore.Identity.Application.Abstractions.Projections;
using RenStore.Identity.Application.Abstractions.Queries;
using RenStore.Identity.Domain.Interfaces;
using RenStore.Identity.Persistence.EventStore;
using RenStore.Identity.Persistence.Outbox;
using RenStore.Identity.Persistence.Read.Queries.Postgresql;
using RenStore.Identity.Persistence.Write.Projections;
using RenStore.Identity.Persistence.Write.Repositories;

namespace RenStore.Identity.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString);
        });
        
        services.AddHostedService<OutboxWorker>();
        
        // dotnet add package Microsoft.Extensions.Options.ConfigurationExtensions
        services.Configure<OutboxOptions>(
            configuration.GetSection(OutboxOptions.SectionName));
        
        services.AddScoped<IIntegrationOutboxWriter, IntegrationOutboxWriter>();
        
        services.AddScoped<IEventStore, SqlEventStore>();
        
        services.AddScoped<IApplicationUserQuery, ApplicationUserQuery>();
        services.AddScoped<IApplicationRoleQuery, ApplicationRoleQuery>();
        
        services.AddScoped<IEmailVerificationProjection, EmailVerificationProjection>();
        services.AddScoped<IRoleProjection, RoleProjection>();
        services.AddScoped<IUserProjection, UserProjection>();
        services.AddScoped<IUserRoleProjection, UserRoleProjection>();
        
        services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        
        return services;
    }
}