namespace RenStore.Catalog.Application.Features.Product.Commands.Approve;

internal sealed class ApproveProductCommandHandler
    : IRequestHandler<ApproveProductCommand>
{
    private readonly ILogger<ApproveProductCommandHandler> _logger;
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _userService;

    public ApproveProductCommandHandler(
        ILogger<ApproveProductCommandHandler> logger,
        IProductRepository productRepository,
        ICurrentUserService userService)
    {
        _logger = logger;
        _productRepository = productRepository;
        _userService = userService;
    }
    
    public async Task Handle(
        ApproveProductCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} for ProductId: {ProductId}",
            nameof(ApproveProductCommand),
            request.ProductId);

        var product = await _productRepository
            .GetAsync(request.ProductId, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                request.ProductId);
        }
        
        product.MarkAsApproved(
            updatedByRole: _userService.Role,
            updatedById: _userService.UserId
                         ?? throw new UnauthorizedException(),
            now: DateTimeOffset.UtcNow);

        await _productRepository.SaveAsync(
            product, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled successfully. ProductId: {ProductId}",
            nameof(ApproveProductCommand),
            request.ProductId);
    }
}