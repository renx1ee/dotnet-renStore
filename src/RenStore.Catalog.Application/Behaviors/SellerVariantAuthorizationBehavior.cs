using RenStore.Catalog.Application.Abstractions;

namespace RenStore.Catalog.Application.Behaviors;

internal sealed class SellerVariantAuthorizationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ISellerVariantCommand
{
    private readonly IProductVariantRepository _variantRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _currentUserService;
    
    public SellerVariantAuthorizationBehavior(
        IProductVariantRepository variantRepository,
        IProductRepository productRepository,
        ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
        _variantRepository = variantRepository;
        _productRepository = productRepository;
    }
    
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (_currentUserService.Role is "Admin" or "Moderator")
            return await next();
        
        var variant = await _variantRepository
            .GetAsync(request.VariantId, cancellationToken);

        if (variant is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Variant.ProductVariant),
                request.VariantId);
        }
        
        var product = await _productRepository
            .GetAsync(id: variant.ProductId, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                variant.ProductId);
        }
        
        if (_currentUserService.UserId != product.SellerId)
        {
            throw new ForbiddenException(
                "You do not have access to this product variant.");
        }
        
        return await next();
    }
}