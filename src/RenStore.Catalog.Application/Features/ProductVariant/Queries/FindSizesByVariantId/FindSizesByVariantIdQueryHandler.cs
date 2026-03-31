using RenStore.Catalog.Application.Service;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindSizesByVariantId;

internal sealed class FindSizesByVariantIdQueryHandler
    : IRequestHandler<FindSizesByVariantIdQuery, IReadOnlyList<VariantSizeReadModel>>
{
    private readonly ILogger<FindSizesByVariantIdQueryHandler> _logger;
    private readonly IProductVariantQuery _variantQuery;
    private readonly IProductQuery _productQuery;
    private readonly IVariantSizeQuery _variantSizeQuery;
    private readonly ICurrentUserService _currentUserService;
    
    public FindSizesByVariantIdQueryHandler(
        ILogger<FindSizesByVariantIdQueryHandler> logger,
        IProductQuery productQuery,
        IProductVariantQuery variantQuery,
        IVariantSizeQuery variantSizeQuery,
        ICurrentUserService currentUserService)
    {
        _logger = logger;
        _productQuery = productQuery;
        _variantQuery = variantQuery;
        _variantSizeQuery = variantSizeQuery;
        _currentUserService = currentUserService;
    }
    
    public async Task<IReadOnlyList<VariantSizeReadModel>> Handle(
        FindSizesByVariantIdQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Query} with VariantId: {VariantId}",
            nameof(FindSizesByVariantIdQuery),
            request.VariantId);

        var sizes = await _variantSizeQuery
            .FindByVariantIdAsync(
                variantId: request.VariantId,
                cancellationToken: cancellationToken);

        if (!sizes.Any()) return [];

        List<VariantSizeReadModel> result;

        if (_currentUserService.Role == Roles.Seller)
        {
            var variant = await _variantQuery.GetByIdAsync(
                id: request.VariantId,
                cancellationToken: cancellationToken);
            
            var product = await _productQuery.GetByIdAsync(
                id: variant.ProductId,
                cancellationToken: cancellationToken);
            
            result = sizes
                .Where(_ => product!.SellerId == _currentUserService.UserId)
                .ToList();
        }
        else
        {
            result = _currentUserService.Role switch
            {
                Roles.Admin or Roles.Moderator or Roles.Support =>
                    sizes.ToList(),
            
                _ => sizes
                    .Where(x => x.IsDeleted == false)
                    .ToList()
            };
        }
        
        _logger.LogInformation(
            "{Query} handled. VariantId: {VariantId}",
            nameof(FindSizesByVariantIdQuery),
            request.VariantId);
        
        return result;
    }
}