using RenStore.Catalog.Application.Abstractions.Services;
using RenStore.Catalog.Application.Service;

namespace RenStore.Catalog.Application.Features.Product.Commands.Archive;

internal sealed class ArchiveProductCommandHandler
    : IRequestHandler<ArchiveProductCommand>
{
    private readonly ILogger<ArchiveProductCommandHandler> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _userService;

    public ArchiveProductCommandHandler(
        ILogger<ArchiveProductCommandHandler> logger,
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
        ArchiveProductCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with ProductId: {ProductId}",
            nameof(ArchiveProductCommand),
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
            
        product.MarkAsArchived(
            updatedByRole: _userService.Role,
            updatedById: _userService.UserId
                         ?? throw new UnauthorizedException(),
            now: now);

        await _productRepository.SaveAsync(
            product, cancellationToken);
        
        await _mediator.Publish(
            new ProductArchivedIntegrationEvent(
                OccurredAt: now,
                ProductId: request.ProductId,
                UpdatedById: _userService.UserId
                             ?? throw new UnauthorizedException(),
                UpdatedByRole: _userService.Role), 
            cancellationToken: cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. ProductId: {ProductId}",
            nameof(ArchiveProductCommand),
            request.ProductId);
    }
}