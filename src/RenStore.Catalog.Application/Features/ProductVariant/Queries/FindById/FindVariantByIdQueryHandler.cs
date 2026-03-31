using RenStore.Catalog.Application.Service;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Catalog.Application.Features.ProductVariant.Queries.FindById;

internal sealed class FindVariantByIdQueryHandler
    : IRequestHandler<FindVariantByIdQuery, ProductVariantReadModel?>
{
    private readonly ILogger<FindVariantByIdQueryHandler> _logger;
    private readonly IProductVariantQuery _variantQuery;
    private readonly IProductQuery _productQuery;
    private readonly ICurrentUserService _currentUserService;
    
    public FindVariantByIdQueryHandler(
        ILogger<FindVariantByIdQueryHandler> logger,
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
        FindVariantByIdQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Query} with VariantId: {VariantId}",
            nameof(FindVariantByIdQuery),
            request.VariantId);

        var variant = await _variantQuery.FindByIdAsync(
            id: request.VariantId,
            cancellationToken: cancellationToken);
        
        if (variant is null) return null;

        if (variant.Status == ProductVariantStatus.Published)
            return variant;

        var product = await _productQuery.GetByIdAsync(
            id: variant.ProductId,
            cancellationToken: cancellationToken);
        
        var result = _currentUserService.Role switch
        {
            Roles.Admin or Roles.Moderator or Roles.Support =>
                variant,

            Roles.Seller =>
                product!.SellerId == _currentUserService.UserId ? variant : null,
            
            _ => null
        };
        
        _logger.LogInformation(
            "{Query} handled. VariantId: {VariantId}",
            nameof(FindVariantByIdQuery),
            request.VariantId);

        return result;
    }
}