using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Catalog.Application.Abstractions.Queries;
using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Domain.Interfaces.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.PublishVariant;

internal sealed class PublishProductVariantCommandHandler
    : IRequestHandler<PublishProductVariantCommand>
{
    private readonly ILogger<PublishProductVariantCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    private readonly IProductQuery _productQuery;
    
    public PublishProductVariantCommandHandler(
        ILogger<PublishProductVariantCommandHandler> logger,
        IProductVariantRepository variantRepository,
        IProductQuery productQuery)
    {
        _logger = logger;
        _variantRepository = variantRepository;
        _productQuery = productQuery;
    }
    
    public async Task Handle(
        PublishProductVariantCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with Variant ID: {VariantId}",
            nameof(PublishProductVariantCommand),
            request.VariantId);

        var variant = await _variantRepository.GetAsync(
            id: request.VariantId,
            cancellationToken: cancellationToken);

        if (variant is null)
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Variant.ProductVariant),
                request.VariantId);
        
        var product = await _productQuery.GetByIdAsync(
            id: variant.ProductId,
            cancellationToken: cancellationToken);

        if (product!.Status != ProductStatus.Published)
            throw new DomainException(
                "You can't publish a variant if the product you own is not published.");

        var now = DateTimeOffset.UtcNow;

        if (variant.MainImageId == Guid.Empty && variant.ImageIds.Any())
        {
            variant.SetMainImageId(now, variant.ImageIds.ToList()[0]);
            // TODO: image.MarkAsMain();
        }
        
        variant.Publish(now);

        await _variantRepository.SaveAsync(variant, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. Variant ID: {VariantId}",
            nameof(PublishProductVariantCommand),
            request.VariantId);
    }
}