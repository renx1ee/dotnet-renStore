using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Catalog.Domain.Interfaces.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.RestoreSize;

internal sealed class RestoreVariantSizeCommandHandler
    : IRequestHandler<RestoreVariantSizeCommand>
{
    private readonly ILogger<RestoreVariantSizeCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    
    public RestoreVariantSizeCommandHandler(
        ILogger<RestoreVariantSizeCommandHandler> logger,
        IProductVariantRepository variantRepository)
    {
        _logger = logger;
        _variantRepository = variantRepository;
    }
    
    public async Task Handle(
        RestoreVariantSizeCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId} and SizeId: {SizeId}",
            nameof(RestoreVariantSizeCommand),
            request.VariantId,
            request.SizeId);

        var variant = await _variantRepository
            .GetAsync(request.VariantId, cancellationToken);

        if (variant is null)
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Variant.ProductVariant),
                request.VariantId);
        
        variant.RestoreSize(
            now: DateTimeOffset.UtcNow, 
            sizeId: request.SizeId);

        await _variantRepository.SaveAsync(variant, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}, SizeId: {SizeId}",
            nameof(RestoreVariantSizeCommand),
            request.VariantId,
            request.SizeId);
    }
}