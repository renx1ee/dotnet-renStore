using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Catalog.Domain.Interfaces.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Application.Features.Product.Commands.Archive;

internal sealed class ArchiveProductCommandHandler
    : IRequestHandler<ArchiveProductCommand>
{
    private readonly ILogger<ArchiveProductCommandHandler> _logger;
    private readonly IProductRepository _productRepository;

    public ArchiveProductCommandHandler(
        ILogger<ArchiveProductCommandHandler> logger,
        IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }
    
    public async Task Handle(
        ArchiveProductCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with ProductId: {ProductId}",
            nameof(ArchiveProductCommand),
            request.ProductId);

        var product = await _productRepository
            .GetAsync(request.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                request.ProductId);
            
        product.MarkAsArchived(DateTimeOffset.UtcNow);

        await _productRepository.SaveAsync(
            product, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. ProductId: {ProductId}",
            nameof(ArchiveProductCommand),
            request.ProductId);
    }
}