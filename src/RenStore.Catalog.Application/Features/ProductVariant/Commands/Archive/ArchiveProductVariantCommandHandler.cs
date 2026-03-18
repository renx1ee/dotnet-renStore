namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Archive;

internal sealed class ArchiveProductVariantCommandHandler
    : IRequestHandler<ArchiveProductVariantCommand>
{
    private readonly ILogger<ArchiveProductVariantCommandHandler> _logger;
    private readonly IProductVariantRepository _productVariantRepository;
    private readonly IProductRepository _productRepository;

    public ArchiveProductVariantCommandHandler(
        ILogger<ArchiveProductVariantCommandHandler> logger,
        IProductVariantRepository productVariantRepository,
        IProductRepository productRepository)
    {
        _logger = logger;
        _productVariantRepository = productVariantRepository;
        _productRepository = productRepository;
    }
    
    public async Task Handle(
        ArchiveProductVariantCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            typeof(ArchiveProductVariantCommand),
            request.VariantId);

        var variant = await _productVariantRepository
            .GetAsync(request.VariantId, cancellationToken)
            ?? throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                request.VariantId);
        
        var product = await _productRepository
            .GetAsync(id: variant.ProductId, cancellationToken) 
            ?? throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                request.VariantId);
        
        if (request.Role == UserRole.Seller &&
            product.SellerId != request.UserId)
        {
            throw new DomainException(nameof(request.UserId));
        }
        
        variant.Archive(
            updatedByRole: request.Role.ToString(),
            updatedById: request.UserId,
            now: DateTimeOffset.UtcNow);

        await _productVariantRepository.SaveAsync(variant, cancellationToken);
            
        _logger.LogInformation(
            "{Command} handled successfully. VariantId: {VariantId}",
            typeof(ArchiveProductVariantCommand),
            request.VariantId);
    }
}