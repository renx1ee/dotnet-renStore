using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindByArticle;

internal sealed class FindVariantByArticleQueryHandler
    : IRequestHandler<FindVariantByArticleQuery, ProductVariantReadModel?>
{
    private readonly ILogger<FindVariantByArticleQueryHandler> _logger;
    private readonly IProductVariantQuery _variantQuery;
    private readonly IProductQuery _productQuery;
    private readonly ICurrentUserService _currentUserService;
    
    public FindVariantByArticleQueryHandler(
        ILogger<FindVariantByArticleQueryHandler> logger,
        IProductVariantQuery variantQuery,
        IProductQuery productQuery,
        ICurrentUserService currentUserService)
    {
        _logger = logger;
        _variantQuery = variantQuery;
        _productQuery = productQuery;
        _currentUserService = currentUserService;
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

        var result = _currentUserService.Role switch
        {
            Roles.Admin or Roles.Moderator =>
                variant,
            
            Roles.Seller =>
                product!.SellerId == _currentUserService.UserId ? variant : null,
            
            _ => null
        };
        
        _logger.LogInformation(
            "{Query} handled. Article: {Article}",
            nameof(FindVariantByArticleQuery),
            request.Article);

        return result;
    }
}