using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Catalog.Application.Abstractions.Queries;
using RenStore.Catalog.Domain.Aggregates.Media;
using RenStore.Catalog.Domain.Interfaces.Repository;
using RenStore.Catalog.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.SetMainImageId;

internal sealed class SetVariantMainImageCommandHandler
    : IRequestHandler<SetVariantMainImageCommand>
{
    private readonly ILogger<SetVariantMainImageCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    private readonly IVariantImageRepository _variantImageRepository;
    
    public SetVariantMainImageCommandHandler(
        ILogger<SetVariantMainImageCommandHandler> logger,
        IProductVariantRepository variantRepository,
        IVariantImageRepository variantImageRepository)
    {
        _logger = logger;
        _variantRepository = variantRepository;
        _variantImageRepository = variantImageRepository;
    }
    
    public async Task Handle(
        SetVariantMainImageCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with Variant ID: {VariantId}, Image ID: {ImageId}",
            nameof(SetVariantMainImageCommand),
            request.VariantId,
            request.ImageId);

        var variant = await _variantRepository.GetAsync(
            id: request.VariantId,
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException(
                name: typeof(Domain.Aggregates.Variant.ProductVariant),
                request.VariantId);
        
        var now = DateTimeOffset.UtcNow;
        
        var currentMainImage = await _variantImageRepository.GetAsync(
            id: variant.MainImageId,
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException(
                name: typeof(VariantImage),
                variant.MainImageId);
        
        currentMainImage.UnsetAsMain(now);
        
        var image = await _variantImageRepository.GetAsync(
            id: request.ImageId,
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException(
                name: typeof(VariantImage),
                request.ImageId);

        image.SetAsMain(now);
        
        variant.SetMainImageId(now, request.ImageId);
        
        await _variantRepository.SaveAsync(variant, cancellationToken);

        await _variantImageRepository.SaveAsync(currentMainImage, cancellationToken);
        await _variantImageRepository.SaveAsync(image, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. Variant ID: {VariantId}, Image ID: {ImageId}",
            nameof(SetVariantMainImageCommand),
            request.VariantId,
            request.ImageId);
    }
}