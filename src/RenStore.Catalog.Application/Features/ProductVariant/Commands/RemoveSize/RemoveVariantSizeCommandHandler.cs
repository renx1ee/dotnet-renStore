using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.AddSize;
using RenStore.Catalog.Domain.Interfaces.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.RemoveSize;

internal sealed class RemoveVariantSizeCommandHandler
    : IRequestHandler<RemoveVariantSizeCommand>
{
    private readonly ILogger<RemoveVariantSizeCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    
    public RemoveVariantSizeCommandHandler(
        ILogger<RemoveVariantSizeCommandHandler> logger,
        IProductVariantRepository variantRepository)
    {
        _logger = logger;
        _variantRepository = variantRepository;
    }
    
    public async Task Handle(
        RemoveVariantSizeCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId} and SizeId: {SizeId}",
            nameof(AddSizeToVariantCommand),
            request.VariantId,
            request.SizeId);

        var variant = await _variantRepository
            .GetAsync(request.VariantId, cancellationToken)
            ?? throw new NotFoundException(
                name: typeof(Domain.Aggregates.Variant.ProductVariant),
                request.VariantId);

        variant.RemoveSize(
            sizeId: request.SizeId,
            now: DateTimeOffset.UtcNow);

        await _variantRepository.SaveAsync(variant, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId} SizeId: {SizeId}",
            nameof(AddSizeToVariantCommand),
            request.VariantId,
            request.SizeId);
    }
}