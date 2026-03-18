namespace RenStore.Catalog.Application.Features.Product.Commands.Reject;

internal sealed class RejectProductCommandHandler
    : IRequestHandler<RejectProductCommand>
{
    private readonly ILogger<RejectProductCommandHandler> _logger;
    private readonly IProductRepository _productRepository;
    
    public RejectProductCommandHandler(
        ILogger<RejectProductCommandHandler> logger,
        IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }
    
    public async Task Handle(
        RejectProductCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with ProductId: {ProductId}",
            nameof(RejectProductCommand),
            request.ProductId);

        var product = await _productRepository.GetAsync(
            request.ProductId, cancellationToken)
            ?? throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                request.ProductId);
        
        product.MarkAsRejected(
            updatedByRole: request.Role.ToString(),
            updatedById: request.UserId,
            now: DateTimeOffset.UtcNow);

        await _productRepository.SaveAsync(
            product, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. ProductId: {ProductId}",
            nameof(RejectProductCommand),
            request.ProductId);
    }
}