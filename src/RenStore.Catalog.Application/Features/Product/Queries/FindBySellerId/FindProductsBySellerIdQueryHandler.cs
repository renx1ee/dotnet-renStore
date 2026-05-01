using RenStore.Catalog.Application.Abstractions.Services;
using RenStore.Catalog.Application.Service;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Catalog.Application.Features.Product.Queries.FindBySellerId;

internal sealed class FindProductsBySellerIdQueryHandler
    : IRequestHandler<FindProductsBySellerIdQuery, IReadOnlyList<ProductReadModel>>
{
    private readonly ILogger<FindProductsBySellerIdQueryHandler> _logger;
    private readonly IProductQuery _productQuery;
    private readonly ICurrentUserService _currentUserService;
    
    public FindProductsBySellerIdQueryHandler(
        ILogger<FindProductsBySellerIdQueryHandler> logger,
        IProductQuery productQuery,
        ICurrentUserService currentUserService)
    {
        _logger = logger;
        _productQuery = productQuery;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<ProductReadModel>> Handle(
        FindProductsBySellerIdQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Query} with SellerId: {SellerId}",
            nameof(FindProductsBySellerIdQuery),
            request.SellerId);

        var products = await _productQuery
            .FindBySellerIdAsync(
                sellerId: request.SellerId,
                sortBy: request.SortBy,
                page: request.Page,
                pageCount: request.PageCount,
                descending: request.Descending,
                isDeleted: request.IsDeleted,
                cancellationToken: cancellationToken);
        
        var result = _currentUserService.Role switch
        {
            Roles.Admin or Roles.Moderator or Roles.Support =>
                products,
            
            Roles.Seller => products
                .Where(x => x.SellerId == _currentUserService.UserId)
                .ToList(),
                
            _ => products
                .Where(x => x.Status == ProductStatus.Published)
        };
        
        _logger.LogInformation(
            "{Query} handled. SellerId: {SellerId}",
            nameof(FindProductsBySellerIdQuery),
            request.SellerId);

        return result.ToList();
    }
}