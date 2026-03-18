namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.SoftDelete;

internal sealed class SoftDeleteProductVariantCommandHandler
    : IRequestHandler<SoftDeleteProductVariantCommand>
{
    private readonly ILogger<SoftDeleteProductVariantCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    private readonly IProductRepository _productRepository;
    
    public SoftDeleteProductVariantCommandHandler(
        ILogger<SoftDeleteProductVariantCommandHandler> logger,
        IProductVariantRepository variantRepository,
        IProductRepository productRepository)
    {
        _logger = logger;
        _variantRepository = variantRepository;
        _productRepository = productRepository;
    }
    
    public async Task Handle(
        SoftDeleteProductVariantCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            nameof(SoftDeleteProductVariantCommand),
            request.VariantId);
        
        var variant = await _variantRepository
            .GetAsync(request.VariantId, cancellationToken)
            ?? throw new NotFoundException(
                name: typeof(Domain.Aggregates.Variant.ProductVariant),
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

        variant.Delete(
            updatedByRole: request.Role.ToString(),
            updatedById: request.UserId,
            now: DateTimeOffset.UtcNow);

        await _variantRepository.SaveAsync(variant, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}",
            nameof(SoftDeleteProductVariantCommand),
            request.VariantId);
    }
}