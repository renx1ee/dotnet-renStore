namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.RestoreSize;

internal sealed class RestoreVariantSizeCommandHandler
    : IRequestHandler<RestoreVariantSizeCommand>
{
    private readonly ILogger<RestoreVariantSizeCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    private readonly IProductRepository _productRepository;
    
    public RestoreVariantSizeCommandHandler(
        ILogger<RestoreVariantSizeCommandHandler> logger,
        IProductVariantRepository variantRepository,
        IProductRepository productRepository)
    {
        _logger = logger;
        _variantRepository = variantRepository;
        _productRepository = productRepository;
    }
    
    public async Task Handle(
        RestoreVariantSizeCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId} and SizeId: {SizeId}",
            nameof(RestoreVariantSizeCommand),
            request.VariantId,
            request.SizeId);

        var variant = await _variantRepository
            .GetAsync(request.VariantId, cancellationToken);

        if (variant is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                request.VariantId);
        }
        
        var product = await _productRepository
            .GetAsync(variant.ProductId, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                variant.ProductId);
        }
        
        variant.RestoreSize(
            updatedByRole: request.Role.ToString(),
            updatedById: request.UserId,
            now: DateTimeOffset.UtcNow, 
            sizeId: request.SizeId);

        await _variantRepository.SaveAsync(variant, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}, SizeId: {SizeId}",
            nameof(RestoreVariantSizeCommand),
            request.VariantId,
            request.SizeId);
    }
}