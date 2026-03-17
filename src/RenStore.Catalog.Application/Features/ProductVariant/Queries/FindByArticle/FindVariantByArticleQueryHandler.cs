namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindByArticle;

internal sealed class FindVariantByArticleQueryHandler
    : IRequestHandler<FindVariantByArticleQuery, ProductVariantReadModel?>
{
    private readonly ILogger<FindVariantByArticleQueryHandler> _logger;
    private readonly IProductVariantQuery _variantQuery;
    private readonly IProductQuery _productQuery;
    
    public FindVariantByArticleQueryHandler(
        ILogger<FindVariantByArticleQueryHandler> logger,
        IProductVariantQuery variantQuery,
        IProductQuery productQuery)
    {
        _logger = logger;
        _variantQuery = variantQuery;
        _productQuery = productQuery;
    }
    
    public async Task<ProductVariantReadModel?> Handle(
        FindVariantByArticleQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Query} with Article: {Article}",
            nameof(FindVariantByArticleQuery),
            request.Article);

        var variant = await _variantQuery.FindByArticleAsync(
            article: request.Article,
            cancellationToken: cancellationToken);

        if (variant is null) return null;

        if (variant.Status == ProductVariantStatus.Published)
            return variant;
        
        var product = await _productQuery.GetByIdAsync(
            id: variant.ProductId,
            cancellationToken: cancellationToken);

        var result = request.Role switch
        {
            UserRole.Admin or UserRole.Moderator =>
                variant,
            
            UserRole.Seller =>
                product!.SellerId == request.UserId ? variant : null,
            
            _ => null
        };
        
        _logger.LogInformation(
            "{Query} handled. Article: {Article}",
            nameof(FindVariantByArticleQuery),
            request.Article);

        return result;
    }
}