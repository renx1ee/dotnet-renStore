namespace RenStore.Catalog.Application.Features.Product.Commands.SoftDelete;

internal sealed class SoftDeleteProductCommandHandler
    : IRequestHandler<SoftDeleteProductCommand>
{
    private readonly ILogger<SoftDeleteProductCommandHandler> _logger;
    private readonly IProductRepository _productRepository;
    
    public SoftDeleteProductCommandHandler(
        ILogger<SoftDeleteProductCommandHandler> logger,
        IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }
    
    public async Task Handle(
        SoftDeleteProductCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {CommandName} for ProductId: {ProductId}",
            nameof(SoftDeleteProductCommand),
            request.ProductId);

        var product = await _productRepository.GetAsync(
            request.ProductId, cancellationToken)
        ?? throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product), 
                request.ProductId);
        
        if (request.Role == UserRole.Seller &&
            product.SellerId != request.UserId)
        {
            throw new DomainException(nameof(request.UserId));
        }
        
        product.Delete(
            updatedByRole: request.Role.ToString(),
            updatedById: request.UserId,
            now: DateTimeOffset.UtcNow);

        await _productRepository.SaveAsync(
            product, cancellationToken);
        
        _logger.LogInformation(
            "{CommandName} handled successfully. ProductId: {ProductId}",
            nameof(SoftDeleteProductCommand),
            request.ProductId);
    }
}