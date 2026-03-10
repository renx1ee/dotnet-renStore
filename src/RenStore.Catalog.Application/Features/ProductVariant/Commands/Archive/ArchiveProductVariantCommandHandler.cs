using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Catalog.Domain.Interfaces.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Archive;

internal sealed class ArchiveProductVariantCommandHandler
    : IRequestHandler<ArchiveProductVariantCommand>
{
    private readonly ILogger<ArchiveProductVariantCommandHandler> _logger;
    private readonly IProductVariantRepository _productVariantRepository;

    public ArchiveProductVariantCommandHandler(
        ILogger<ArchiveProductVariantCommandHandler> logger,
        IProductVariantRepository productVariantRepository)
    {
        _logger = logger;
        _productVariantRepository = productVariantRepository;
    }
    
    public async Task Handle(
        ArchiveProductVariantCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            typeof(ArchiveProductVariantCommand),
            request.VariantId);

        var variant = await _productVariantRepository
            .GetAsync(request.VariantId, cancellationToken);

        if (variant is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                request.VariantId);
        }
        
        variant.Archive(DateTimeOffset.UtcNow);

        await _productVariantRepository.SaveAsync(variant, cancellationToken);
            
        _logger.LogInformation(
            "{Command} handled successfully. VariantId: {VariantId}",
            typeof(ArchiveProductVariantCommand),
            request.VariantId);
    }
}