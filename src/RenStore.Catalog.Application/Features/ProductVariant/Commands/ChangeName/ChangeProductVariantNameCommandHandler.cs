namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.ChangeName;

internal sealed class ChangeProductVariantNameCommandHandler
    : IRequestHandler<ChangeProductVariantNameCommand>
{
    private readonly ILogger<ChangeProductVariantNameCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    private readonly IProductRepository _productRepository;
    
    public ChangeProductVariantNameCommandHandler(
        ILogger<ChangeProductVariantNameCommandHandler> logger,
        IProductVariantRepository variantRepository,
        IProductRepository productRepository)
    {
        _logger = logger;
        _variantRepository = variantRepository;
        _productRepository = productRepository;
    }
    
    public async Task Handle(
        ChangeProductVariantNameCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            nameof(ChangeProductVariantNameCommand),
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
        
        if (product.SellerId != request.UserId)
        {
            throw new DomainException(nameof(request.UserId));
        }

        variant.ChangeName(
            now: DateTimeOffset.UtcNow,
            name: request.Name);

        await _variantRepository.SaveAsync(variant, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}",
            nameof(ChangeProductVariantNameCommand),
            request.VariantId);
    }
}