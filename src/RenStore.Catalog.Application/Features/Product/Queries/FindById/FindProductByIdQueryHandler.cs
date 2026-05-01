using RenStore.Catalog.Application.Abstractions.Services;
using RenStore.Catalog.Application.Service;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Catalog.Application.Features.Product.Queries.FindById;

internal sealed class FindProductByIdQueryHandler
    : IRequestHandler<FindProductByIdQuery, ProductReadModel?>
{
    private readonly ILogger<FindProductByIdQueryHandler> _logger;
    private readonly IProductQuery _productQuery;
    private readonly ICurrentUserService _currentUserService;
    
    public FindProductByIdQueryHandler(
        ILogger<FindProductByIdQueryHandler> logger,
        IProductQuery productQuery,
        ICurrentUserService currentUserService)
    {
        _logger = logger;
        _productQuery = productQuery;
        _currentUserService = currentUserService;
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

        var result = _currentUserService.Role switch
        {
            Roles.Admin or Roles.Moderator or Roles.Support =>
                product,

            Roles.Seller =>
                product.SellerId == _currentUserService.UserId ? product : null,
            
            _ => null
        };
        
        _logger.LogInformation(
            "{Query} handled. ProductId: {ProductId}",
            nameof(FindProductByIdQuery),
            request.ProductId);

        return result;
    }
}