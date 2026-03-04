using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Catalog.Application.Abstractions.Projections;
using RenStore.Catalog.Domain.Interfaces.Repository;

namespace RenStore.Catalog.Application.Features.Product.Commands.Create;

internal sealed class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly ILogger<CreateProductCommandHandler> _logger;
    private readonly IProductRepository _productRepository;
    private readonly ICategoryProjection _categoryProjection;
    
    public CreateProductCommandHandler(
        ILogger<CreateProductCommandHandler> logger,
        IProductRepository productRepository,
        ICategoryProjection categoryProjection)
    {
        _logger = logger;
        _productRepository = productRepository;
        _categoryProjection = categoryProjection;
    }
    
    public async Task<Guid> Handle(
        CreateProductCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {CommandName} for SellerId {SellerId} SubCategoryId: {SubCategoryId}", 
            nameof(CreateProductCommand),
            request.SellerId,
            request.SubCategoryId);

        var existingCategory = await _categoryProjection
            .SubCategoryExists(
                request.SubCategoryId, 
                cancellationToken);

        // TODO:
        if (existingCategory)
        {
            /*throw new NotFoundException(
                name: typeof(SubCategoryReadModel), 
                request.SubCategoryId);*/
            
            return Guid.Empty;
        }

        var product = Domain.Aggregates.Product.Product.Create(
            sellerId: request.SellerId,
            subCategoryId: request.SubCategoryId,
            now: DateTimeOffset.UtcNow);

        await _productRepository.SaveAsync(
            product, cancellationToken);

        _logger.LogInformation(
            "{CommandName} handled successfully. ProductId: {ProductId}",
            nameof(CreateProductCommand),
            product.Id);
        
        return product.Id;
    }
}