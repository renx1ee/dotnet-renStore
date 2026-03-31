using RenStore.Catalog.Application.Service;

namespace RenStore.Catalog.Application.Features.Product.Commands.SoftDelete;

internal sealed class SoftDeleteProductCommandHandler
    : IRequestHandler<SoftDeleteProductCommand>
{
    private readonly ILogger<SoftDeleteProductCommandHandler> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _userService;
    
    public SoftDeleteProductCommandHandler(
        ILogger<SoftDeleteProductCommandHandler> logger,
        IProductRepository productRepository,
        IMediator mediator,
        ICurrentUserService userService)
    {
        _logger = logger;
        _productRepository = productRepository;
        _mediator = mediator;
        _userService = userService;
    }
    
    public async Task Handle(
        SoftDeleteProductCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {CommandName} for ProductId: {ProductId}",
            nameof(SoftDeleteProductCommand),
            request.ProductId);

        var product = await _productRepository
            .GetAsync(request.ProductId, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                request.ProductId);
        }

        var now = DateTimeOffset.UtcNow;
        var userId = _userService.UserId ?? throw new UnauthorizedException();
        
        product.Delete(
            updatedByRole: _userService.Role,
            updatedById: userId,
            now: now);

        await _productRepository.SaveAsync(
            product, cancellationToken);

        await _mediator.Publish(
            new ProductDeletedIntegrationEvent(
                OccurredAt: now,
                ProductId: request.ProductId,
                UpdatedById: userId,
                UpdatedByRole: _userService.Role), 
                cancellationToken: cancellationToken);
        
        _logger.LogInformation(
            "{CommandName} handled successfully. ProductId: {ProductId}",
            nameof(SoftDeleteProductCommand),
            request.ProductId);
    }
}