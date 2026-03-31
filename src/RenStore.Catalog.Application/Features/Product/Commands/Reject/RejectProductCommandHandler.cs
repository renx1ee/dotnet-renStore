using RenStore.Catalog.Application.Service;

namespace RenStore.Catalog.Application.Features.Product.Commands.Reject;

internal sealed class RejectProductCommandHandler
    : IRequestHandler<RejectProductCommand>
{
    private readonly ILogger<RejectProductCommandHandler> _logger;
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _userService;
    
    public RejectProductCommandHandler(
        ILogger<RejectProductCommandHandler> logger,
        IProductRepository productRepository,
        ICurrentUserService userService)
    {
        _logger = logger;
        _productRepository = productRepository;
        _userService = userService;
    }
    
    public async Task Handle(
        RejectProductCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with ProductId: {ProductId}",
            nameof(RejectProductCommand),
            request.ProductId);

        var product = await _productRepository
            .GetAsync(request.ProductId, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                request.ProductId);
        }
        
        product.MarkAsRejected(
            updatedByRole: _userService.Role,
            updatedById: _userService.UserId
                         ?? throw new UnauthorizedException(),
            now: DateTimeOffset.UtcNow);

        await _productRepository.SaveAsync(
            product, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. ProductId: {ProductId}",
            nameof(RejectProductCommand),
            request.ProductId);
    }
}