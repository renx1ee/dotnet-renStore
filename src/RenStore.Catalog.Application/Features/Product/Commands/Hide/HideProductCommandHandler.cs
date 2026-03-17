namespace RenStore.Catalog.Application.Features.Product.Commands.Hide;

internal sealed class HideProductCommandHandler
    : IRequestHandler<HideProductCommand>
{
    private readonly ILogger<HideProductCommandHandler> _logger;
    private readonly IProductRepository _productRepository;

    public HideProductCommandHandler(
        ILogger<HideProductCommandHandler> logger,
        IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
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
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                request.ProductId);
            
        product.MarkAsHidden(DateTimeOffset.UtcNow);

        await _productRepository.SaveAsync(
            product, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. ProductId: {ProductId}",
            nameof(HideProductCommand),
            request.ProductId);
    }
}