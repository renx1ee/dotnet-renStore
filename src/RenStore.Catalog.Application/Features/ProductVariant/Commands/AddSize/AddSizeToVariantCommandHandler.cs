namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.AddSize;

internal sealed class AddSizeToVariantCommandHandler
    : IRequestHandler<AddSizeToVariantCommand, Guid>
{
    private readonly ILogger<AddSizeToVariantCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    private readonly IProductRepository _productRepository;
    
    public AddSizeToVariantCommandHandler(
        ILogger<AddSizeToVariantCommandHandler> logger,
        IProductVariantRepository variantRepository,
        IProductRepository productRepository,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _variantRepository = variantRepository;
        _productRepository = productRepository;
    }
    
    public async Task<Guid> Handle(
        AddSizeToVariantCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            nameof(AddSizeToVariantCommand),
            request.VariantId);

        var variant = await _variantRepository
            .GetAsync(request.VariantId, cancellationToken);

        if (variant is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Variant.ProductVariant),
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

        var sizeId = variant.AddSize(
            letterSize: request.LetterSize,
            now: DateTimeOffset.UtcNow);

        await _variantRepository.SaveAsync(variant, cancellationToken);

        /*await _publishEndpoint.Publish(
            new VariantSizeCreatedIntegrationEvent(
                VariantId: request.VariantId,
                SizeId: sizeId),
            cancellationToken);*/
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}",
            nameof(AddSizeToVariantCommand),
            request.VariantId);
        
        return sizeId;
    }
}