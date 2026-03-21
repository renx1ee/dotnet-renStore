namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.AddPrice;

public class AddPriceToVariantSizeCommandHandler
    : IRequestHandler<AddPriceToVariantSizeCommand>
{
    private readonly ILogger<AddPriceToVariantSizeCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    private readonly IProductRepository _productRepository;
    
    public AddPriceToVariantSizeCommandHandler(
        ILogger<AddPriceToVariantSizeCommandHandler> logger,
        IProductVariantRepository variantRepository,
        IProductRepository productRepository)
    {
        _logger = logger;
        _variantRepository = variantRepository;
        _productRepository = productRepository;
    }
    
    public async Task Handle(
        AddPriceToVariantSizeCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId} and SizeId: {SizeId}",
            nameof(AddPriceToVariantSizeCommand),
            request.VariantId,
            request.SizeId);
        
        var variant = await _variantRepository
            .GetAsync(request.VariantId, cancellationToken);
        
        if (variant is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Variant.ProductVariant),
                request.VariantId);
        }
        
        // TODO: вынести в pipeline
        var product = await _productRepository
            .GetAsync(variant.ProductId, cancellationToken);
        
        if (product is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                variant.ProductId);
        }
        
        if (product.Status == ProductStatus.Deleted)
            throw new DomainException(
                "Cannot add price to already deleted product.");

        variant.AddPriceToSize(
            amount: request.Price,
            currency: request.Currency,
            validFrom: request.ValidFrom,
            now: DateTimeOffset.UtcNow,
            sizeId: request.SizeId);

        await _variantRepository.SaveAsync(variant, cancellationToken);

        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}, SizeId: {SizeId}",
            nameof(AddPriceToVariantSizeCommand),
            request.VariantId,
            request.SizeId);
    }
}