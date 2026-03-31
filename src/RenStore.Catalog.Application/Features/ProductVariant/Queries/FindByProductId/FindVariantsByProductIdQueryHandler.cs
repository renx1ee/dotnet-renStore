using RenStore.Catalog.Application.Service;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindByProductId;

internal sealed class FindVariantsByProductIdQueryHandler
    : IRequestHandler<FindVariantsByProductIdQuery, IReadOnlyList<ProductVariantReadModel>>
{
    private readonly ILogger<FindVariantsByProductIdQueryHandler> _logger;
    private readonly IProductVariantQuery _variantQuery;
    private readonly IProductQuery _productQuery;
    private readonly ICurrentUserService _currentUserService;
    
    public FindVariantsByProductIdQueryHandler(
        ILogger<FindVariantsByProductIdQueryHandler> logger,
        IProductVariantQuery variantQuery,
        IProductQuery productQuery,
        ICurrentUserService currentUserService)
    {
        _logger = logger;
        _variantQuery = variantQuery;
        _productQuery = productQuery;
        _currentUserService = currentUserService;
    }
    
    public async Task<IReadOnlyList<ProductVariantReadModel>> Handle(
        FindVariantsByProductIdQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Query} with ProductId: {ProductId}",
            nameof(FindVariantsByProductIdQuery),
            request.ProductId);
        
        var product = await _productQuery
            .GetByIdAsync(
                id: request.ProductId,
                cancellationToken: cancellationToken);

        var variants = await _variantQuery
            .FindByProductIdAsync(
                productId: request.ProductId,
                sortBy: request.SortBy,
                page: request.Page,
                pageSize: request.PageCount,
                descending: request.Descending,
                isDeleted: request.IsDeleted,
                cancellationToken: cancellationToken);

        if (!variants.Any()) return [];

        var result = _currentUserService.Role switch
        {
            Roles.Admin or Roles.Moderator or Roles.Support =>
                variants,
            
            Roles.Seller => variants
                .Where(_ => product!.SellerId == _currentUserService.UserId)
                .ToList(),
            
            _ => variants
                .Where(x => x.Status == ProductVariantStatus.Published)
                .ToList()
        };
        
        _logger.LogInformation(
            "{Query} handled. ProductId: {ProductId}",
            nameof(FindVariantsByProductIdQuery),
            request.ProductId);
        
        return result;
    }
}