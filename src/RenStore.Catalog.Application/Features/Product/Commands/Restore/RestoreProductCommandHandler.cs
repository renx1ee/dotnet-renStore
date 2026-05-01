using RenStore.Catalog.Application.Abstractions.Services;
using RenStore.Catalog.Application.Service;

namespace RenStore.Catalog.Application.Features.Product.Commands.Restore;

internal sealed class RestoreProductCommandHandler
    : IRequestHandler<RestoreProductCommand>
{
    private readonly ILogger<RestoreProductCommandHandler> _logger;
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _userService;
    
    public RestoreProductCommandHandler(
        ILogger<RestoreProductCommandHandler> logger,
        IProductRepository productRepository,
        ICurrentUserService userService)
    {
        _logger = logger;
        _productRepository = productRepository;
        _userService = userService;
    }
    
    public async Task Handle(
        RestoreProductCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with ProductId: {ProductId}",
            nameof(RestoreProductCommand),
            request.ProductId);

        var product = await _productRepository
            .GetAsync(request.ProductId, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                request.ProductId);
        }
        
        product.Restore(
            updatedByRole: _userService.Role,
            updatedById: _userService.UserId
                         ?? throw new UnauthorizedException(),
            now: DateTimeOffset.UtcNow);

        await _productRepository.SaveAsync(product, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. ProductId: {ProductId}",
            nameof(RestoreProductCommand),
            request.ProductId);
    }
}