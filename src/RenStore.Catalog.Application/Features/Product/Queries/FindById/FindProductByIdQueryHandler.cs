using RenStore.Catalog.Application.Abstractions.Queries;
using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Application.Features.Product.Queries.FindById;

internal sealed class FindProductByIdQueryHandler
    : IRequestHandler<FindProductByIdQuery, ProductReadModel?>
{
    private readonly ILogger<FindProductByIdQueryHandler> _logger;
    private readonly IProductQuery _productQuery;
    
    public FindProductByIdQueryHandler(
        ILogger<FindProductByIdQueryHandler> logger,
        IProductQuery productQuery)
    {
        _logger = logger;
        _productQuery = productQuery;
    }
    
    public async Task<ProductReadModel?> Handle(
        FindProductByIdQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Query} with ProductId: {ProductId}",
            nameof(FindProductByIdQuery),
            request.ProductId);

        var product = await _productQuery
            .FindByIdAsync(
                id: request.ProductId,
                cancellationToken: cancellationToken);

        if (product is null) return null;

        if (product.Status == ProductStatus.Published)
            return product;

        var result = request.Role switch
        {
            UserRole.Admin or UserRole.Moderator or UserRole.Support =>
                product,

            UserRole.Seller =>
                product.SellerId == request.UserId ? product : null,
            
            _ => null
        };
        
        _logger.LogInformation(
            "{Query} handled. ProductId: {ProductId}",
            nameof(FindProductByIdQuery),
            request.ProductId);

        return result;
    }
}