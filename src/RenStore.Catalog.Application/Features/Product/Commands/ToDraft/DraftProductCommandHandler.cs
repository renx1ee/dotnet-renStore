namespace RenStore.Catalog.Application.Features.Product.Commands.ToDraft;

internal sealed class DraftProductCommandHandler
    : IRequestHandler<DraftProductCommand>
{
    private readonly ILogger<DraftProductCommandHandler> _logger;
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _userService;

    public DraftProductCommandHandler(
        ILogger<DraftProductCommandHandler> logger,
        IProductRepository productRepository,
        ICurrentUserService userService)
    {
        _logger = logger;
        _productRepository = productRepository;
        _userService = userService;
    }
    
    public async Task Handle(
        DraftProductCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {CommandName} for ProductId: {ProductId}",
            nameof(DraftProductCommand),
            request.ProductId);

        var product = await _productRepository
            .GetAsync(request.ProductId, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                request.ProductId);
        }
        
        product.MarkAsDraft(
            updatedByRole: _userService.Role,
            updatedById: _userService.UserId
                         ?? throw new UnauthorizedException(),
            now: DateTimeOffset.UtcNow);

        await _productRepository.SaveAsync(
            product, cancellationToken);
        
        _logger.LogInformation(
            "{CommandName} handled successfully. ProductId: {ProductId}",
            nameof(DraftProductCommand),
            request.ProductId);
    }
}