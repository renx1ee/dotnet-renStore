namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindSizesByVariantId;

internal sealed class FindSizesByVariantIdQueryHandler
    : IRequestHandler<FindSizesByVariantIdQuery, IReadOnlyList<VariantSizeReadModel>>
{
    private readonly ILogger<FindSizesByVariantIdQueryHandler> _logger;
    private readonly IProductVariantQuery _variantQuery;
    private readonly IProductQuery _productQuery;
    private readonly IVariantSizeQuery _variantSizeQuery;
    
    public FindSizesByVariantIdQueryHandler(
        ILogger<FindSizesByVariantIdQueryHandler> logger,
        IProductQuery productQuery,
        IProductVariantQuery variantQuery,
        IVariantSizeQuery variantSizeQuery)
    {
        _logger = logger;
        _productQuery = productQuery;
        _variantQuery = variantQuery;
        _variantSizeQuery = variantSizeQuery;
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

        if (request.Role == UserRole.Seller)
        {
            var variant = await _variantQuery.GetByIdAsync(
                id: request.VariantId,
                cancellationToken: cancellationToken);
            
            var product = await _productQuery.GetByIdAsync(
                id: variant.ProductId,
                cancellationToken: cancellationToken);
            
            result = sizes
                .Where(_ => product!.SellerId == request.UserId)
                .ToList();
        }
        else
        {
            result = request.Role switch
            {
                UserRole.Admin or UserRole.Moderator or UserRole.Support =>
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