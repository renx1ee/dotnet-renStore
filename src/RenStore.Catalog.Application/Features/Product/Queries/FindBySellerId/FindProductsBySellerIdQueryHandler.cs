using RenStore.Catalog.Application.Abstractions.Queries;
using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Application.Features.Product.Queries.FindBySellerId;

internal sealed class FindProductsBySellerIdQueryHandler
    : IRequestHandler<FindProductsBySellerIdQuery, IReadOnlyList<ProductReadModel>>
{
    private readonly ILogger<FindProductsBySellerIdQueryHandler> _logger;
    private readonly IProductQuery _productQuery;
    
    public FindProductsBySellerIdQueryHandler(
        ILogger<FindProductsBySellerIdQueryHandler> logger,
        IProductQuery productQuery)
    {
        _logger = logger;
        _productQuery = productQuery;
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
        
        var result = request.Role switch
        {
            UserRole.Admin or UserRole.Moderator or UserRole.Support =>
                products,
            
            UserRole.Seller => products
                .Where(x => x.SellerId == request.UserId)
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