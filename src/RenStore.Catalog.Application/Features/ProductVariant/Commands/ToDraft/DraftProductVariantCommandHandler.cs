namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.ToDraft;

internal sealed class DraftProductVariantCommandHandler
    : IRequestHandler<DraftProductVariantCommand>
{
    private readonly ILogger<DraftProductVariantCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    private readonly IProductRepository _productRepository;
    
    public DraftProductVariantCommandHandler(
        ILogger<DraftProductVariantCommandHandler> logger,
        IProductVariantRepository variantRepository,
        IProductRepository productRepository)
    {
        _logger = logger;
        _variantRepository = variantRepository;
        _productRepository = productRepository;
    }
    
    public async Task Handle(
        DraftProductVariantCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            typeof(DraftProductVariantCommand),
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

        variant.MarkAsDraft(
            updatedByRole: request.Role.ToString(),
            updatedById: request.UserId,
            now: DateTimeOffset.UtcNow);

        await _variantRepository.SaveAsync(variant, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}",
            typeof(DraftProductVariantCommand),
            request.VariantId);
    }
}