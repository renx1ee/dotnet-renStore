using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RenStore.Catalog.Application.Behaviors;
using RenStore.Catalog.Application.Features.Product.Commands.Approve;
using RenStore.Catalog.Application.Features.Product.Commands.Archive;
using RenStore.Catalog.Application.Features.Product.Commands.Create;
using RenStore.Catalog.Application.Features.Product.Commands.Hide;
using RenStore.Catalog.Application.Features.Product.Commands.Reject;
using RenStore.Catalog.Application.Features.Product.Commands.SoftDelete;
using RenStore.Catalog.Application.Features.Product.Commands.ToDraft;

namespace RenStore.Catalog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogApplication(
        this IServiceCollection services)
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
                typeof(CreateProductCommandHandler).Assembly,
                typeof(SoftDeleteProductCommand).Assembly,
                typeof(SoftDeleteProductCommandHandler).Assembly,
                typeof(ApproveProductCommand).Assembly,
                typeof(ApproveProductCommandHandler).Assembly,
                typeof(ArchiveProductCommand).Assembly,
                typeof(ArchiveProductCommandHandler).Assembly,
                typeof(HideProductCommand).Assembly,
                typeof(HideProductCommandHandler).Assembly,
                typeof(DraftProductCommand).Assembly,
                typeof(DraftProductCommandHandler).Assembly,
                typeof(RejectProductCommand).Assembly,
                typeof(RejectProductCommandHandler).Assembly));

        services.AddValidatorsFromAssembly(
            typeof(CreateProductCommandValidator).Assembly);
        
        services.AddValidatorsFromAssembly(
            typeof(SoftDeleteProductCommandValidator).Assembly);
        
        services.AddValidatorsFromAssembly(
            typeof(ApproveProductCommandValidator).Assembly);
        
        services.AddValidatorsFromAssembly(
            typeof(RejectProductCommandValidator).Assembly);
        
        services.AddValidatorsFromAssembly(
            typeof(ArchiveProductCommandValidator).Assembly);
        
        services.AddValidatorsFromAssembly(
            typeof(HideProductCommandValidator).Assembly);
        
        services.AddValidatorsFromAssembly(
            typeof(DraftProductCommandValidator).Assembly);
        
        return services;
    }
}