using RenStore.Catalog.Application.Service;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindByUrlSlug;

internal sealed class FindByUrlSlugQueryHandler
    : IRequestHandler<FindByUrlSlugQuery, ProductVariantReadModel?>
{
    private readonly ILogger<FindByUrlSlugQueryHandler> _logger;
    private readonly IProductVariantQuery _variantQuery;
    private readonly IProductQuery _productQuery;
    private readonly ICurrentUserService _currentUserService;

    public FindByUrlSlugQueryHandler(
        ILogger<FindByUrlSlugQueryHandler> logger,
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
        FindByUrlSlugQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Query} with UrlSlug: {UrlSlug}",
            nameof(FindByUrlSlugQuery),
            request.UrlSlug);

        var variant = await _variantQuery.FindByUrlSlugAsync(
            urlSlug: request.UrlSlug,
            cancellationToken: cancellationToken);

        if (variant is null) return null;

        if (variant.Status == ProductVariantStatus.Published)
            return variant;

        /*var product = await _productQuery.GetByIdAsync(
            id: variant.ProductId,
            cancellationToken: cancellationToken);*/

        var result = _currentUserService.Role switch
        {
            Roles.Admin or Roles.Moderator =>
                variant,

            /*Roles.Seller =>
                product!.SellerId == _currentUserService.UserId ? variant : null,*/

            _ => null
        };

        _logger.LogInformation(
            "{Query} handled. UrlSlug: {UrlSlug}",
            nameof(FindByUrlSlugQuery),
            request.UrlSlug);

        return result;
    }
}
