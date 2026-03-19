using RenStore.Catalog.Domain.Aggregates.Media;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.SetMainImageId;

internal sealed class SetVariantMainImageCommandHandler
    : IRequestHandler<SetVariantMainImageCommand>
{
    private readonly ILogger<SetVariantMainImageCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    private readonly IVariantImageRepository _variantImageRepository;
    private readonly IProductRepository _productRepository;
    
    public SetVariantMainImageCommandHandler(
        ILogger<SetVariantMainImageCommandHandler> logger,
        IProductVariantRepository variantRepository,
        IVariantImageRepository variantImageRepository,
        IProductRepository productRepository)
    {
        _logger = logger;
        _variantRepository = variantRepository;
        _variantImageRepository = variantImageRepository;
        _productRepository = productRepository;
    }
    
    public async Task Handle(
        SetVariantMainImageCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with Variant ID: {VariantId}, Image ID: {ImageId}",
            nameof(SetVariantMainImageCommand),
            request.VariantId,
            request.ImageId);

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
        
        if (product.SellerId != request.UserId)
        {
            throw new DomainException(nameof(request.UserId));
        }
        
        var now = DateTimeOffset.UtcNow;
        
        var currentMainImage = await _variantImageRepository.GetAsync(
            id: variant.MainImageId,
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException(
                name: typeof(VariantImage),
                variant.MainImageId);
        
        currentMainImage.UnsetAsMain(now);
        
        var image = await _variantImageRepository.GetAsync(
            id: request.ImageId,
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException(
                name: typeof(VariantImage),
                request.ImageId);

        image.SetAsMain(now);
        
        variant.SetMainImageId(now, request.ImageId);
        
        await _variantRepository.SaveAsync(variant, cancellationToken);

        await _variantImageRepository.SaveAsync(currentMainImage, cancellationToken);
        await _variantImageRepository.SaveAsync(image, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. Variant ID: {VariantId}, Image ID: {ImageId}",
            nameof(SetVariantMainImageCommand),
            request.VariantId,
            request.ImageId);
    }
}