using RenStore.Catalog.Application.Abstractions.Services;
using RenStore.Catalog.Application.Service;

namespace RenStore.Catalog.Application.Features.Product.Commands.Hide;

internal sealed class HideProductCommandHandler
    : IRequestHandler<HideProductCommand>
{
    private readonly ILogger<HideProductCommandHandler> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _userService;

    public HideProductCommandHandler(
        ILogger<HideProductCommandHandler> logger,
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
        HideProductCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with ProductId: {ProductId}",
            nameof(HideProductCommand),
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
            
        product.MarkAsHidden(
            updatedByRole: _userService.Role,
            updatedById: userId,
            now: now);

        await _productRepository.SaveAsync(
            product, cancellationToken);
        
        await _mediator.Publish(
            new ProductHiddenIntegrationEvent(
                OccurredAt: now,
                ProductId: request.ProductId,
                UpdatedById:userId,
                UpdatedByRole: _userService.Role), 
            cancellationToken: cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. ProductId: {ProductId}",
            nameof(HideProductCommand),
            request.ProductId);
    }
}