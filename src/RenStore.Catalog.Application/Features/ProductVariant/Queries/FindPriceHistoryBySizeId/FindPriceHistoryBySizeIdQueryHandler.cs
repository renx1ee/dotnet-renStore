using RenStore.Catalog.Application.Service;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindPriceHistoryBySizeId;

internal sealed class FindPriceHistoryBySizeIdQueryHandler
    : IRequestHandler<FindPriceHistoryBySizeIdQuery, IReadOnlyList<PriceHistoryReadModel>>
{
    private readonly ILogger<FindPriceHistoryBySizeIdQueryHandler> _logger;
    private readonly IProductVariantQuery _variantQuery;
    private readonly IProductQuery _productQuery;
    private readonly IPriceHistoryQuery _priceHistoryQuery;
    private readonly ICurrentUserService _currentUserService;
    
    public FindPriceHistoryBySizeIdQueryHandler(
        ILogger<FindPriceHistoryBySizeIdQueryHandler> logger,
        IProductQuery productQuery,
        IProductVariantQuery variantQuery,
        IPriceHistoryQuery priceHistoryQuery,
        ICurrentUserService currentUserService)
    {
        _logger = logger;
        _productQuery = productQuery;
        _variantQuery = variantQuery;
        _priceHistoryQuery = priceHistoryQuery;
        _currentUserService = currentUserService;
    }
    
    public async Task<IReadOnlyList<PriceHistoryReadModel>> Handle(
        FindPriceHistoryBySizeIdQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Query} with VariantId: {VariantId}, SizeId: {SizeId}",
            nameof(FindPriceHistoryBySizeIdQuery),
            request.VariantId,
            request.SizeId);

        var priceHistory = await _priceHistoryQuery
            .FindBySizeIdAsync(
                sizeId: request.SizeId,
                sortBy: request.SortBy,
                page: request.Page,
                pageSize: request.PageCount,
                descending: request.Descending,
                isActive: request.IsActive,
                cancellationToken: cancellationToken);

        if (!priceHistory.Any()) return [];

        List<PriceHistoryReadModel> result;

        if (_currentUserService.Role == Roles.Seller)
        {
            var variant = await _variantQuery.GetByIdAsync(
                id: request.VariantId,
                cancellationToken: cancellationToken);
            
            var product = await _productQuery.GetByIdAsync(
                id: variant.ProductId,
                cancellationToken: cancellationToken);
            
            result = priceHistory
                .Where(_ => product!.SellerId == _currentUserService.UserId)
                .ToList();
        }
        else
        {
            result = _currentUserService.Role switch
            {
                Roles.Admin or Roles.Moderator or Roles.Support =>
                    priceHistory.ToList(),
            
                _ => priceHistory
                    .Where(x => x.IsActive)
                    .ToList()
            };
        }
        
        _logger.LogInformation(
            "{Query} handled. VariantId: {VariantId}, SizeId: {SizeId}",
            nameof(FindPriceHistoryBySizeIdQuery),
            request.VariantId,
            request.SizeId);
        
        return result;
    }
}