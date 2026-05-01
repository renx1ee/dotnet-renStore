using RenStore.Catalog.Application.Abstractions.Services;
using RenStore.Catalog.Application.Service;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Catalog.Application.Behaviors;

internal sealed class SellerProductAuthorizationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ISellerProductCommand
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IProductRepository _productRepository;

    public SellerProductAuthorizationBehavior(
        ICurrentUserService currentUserService,
        IProductRepository productRepository)
    {
        _currentUserService = currentUserService;
        _productRepository = productRepository;
    }
    
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (_currentUserService.Role is Roles.Admin or Roles.Moderator)
            return await next();
        
        var product = await _productRepository
            .GetAsync(id: request.ProductId, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                request.ProductId);
        }
        
        if (_currentUserService.UserId != product.SellerId)
        {
            throw new ForbiddenException(
                "You do not have access to this product.");   
        }
        
        return await next();
    }
}