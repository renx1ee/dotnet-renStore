using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Catalog.Domain.Aggregates.Media;
using RenStore.Catalog.Domain.DomainService;
using RenStore.Catalog.Domain.Interfaces.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Application.Features.Product.Commands.PublishProduct;

// TODO: проверить и сделать нотификейшенс.
internal sealed class PublishProductCommandHandler
    : IRequestHandler<PublishProductCommand>
{
    private readonly ILogger<PublishProductCommandHandler> _logger;
    private readonly IPublishProductService _productPublishService;
    private readonly IProductRepository _productRepository;
    private readonly IProductVariantRepository _variantRepository;
    private readonly IVariantImageRepository _variantImageRepository;
    
    public PublishProductCommandHandler(
        ILogger<PublishProductCommandHandler> logger,
        IPublishProductService productPublishService,
        IProductRepository productRepository,
        IProductVariantRepository variantRepository,
        IVariantImageRepository variantImageRepository)
    {
        _logger = logger;
        _productPublishService = productPublishService;
        _productRepository = productRepository;
        _variantRepository = variantRepository;
        _variantImageRepository = variantImageRepository;
    }
    
    public async Task Handle(
        PublishProductCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with ProductId: {ProductId}",
            nameof(PublishProductCommand),
            request.ProductId);

        var product = await _productRepository
            .GetAsync(request.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                request.ProductId);

        var variants = new List<Domain.Aggregates.Variant.ProductVariant>();
        var imagesToVariants = new Dictionary<Guid, IReadOnlyCollection<VariantImage>>();

        var now = DateTimeOffset.UtcNow;

        foreach (var variantId in product.ProductVariantIds)
        {
            var variant = await _variantRepository
                .GetAsync(variantId, cancellationToken);

            if (variant is null)
                throw new NotFoundException(
                    name: typeof(Domain.Aggregates.Variant.ProductVariant),
                    variantId);
            
            variants.Add(variant);

            var images = new List<VariantImage>();

            foreach (var imageId in variant.ImageIds)
            {
                var image = await _variantImageRepository
                    .GetAsync(imageId, cancellationToken);
                
                if (image is null)
                    throw new NotFoundException(
                        name: typeof(VariantImage), imageId);
                
                if (variant.MainImageId == Guid.Empty)
                {
                    // TODO:
                    image.SetAsMain(now);
                    
                    if(image.IsMain)
                        variant.SetMainImageId(
                            imageId: image.Id, 
                            now: now);
                }
            
                images.Add(image);
            }
            
            imagesToVariants.Add(variantId, images);
        }
        
        _productPublishService.Publish(
            now: now, 
            product: product,
            variants: variants,
            imagesByVariants: imagesToVariants);

        await _productRepository.SaveAsync(product, cancellationToken);

        foreach (var variant in variants)
            await _variantRepository.SaveAsync(variant, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. ProductId: {ProductId}",
            nameof(PublishProductCommand),
            request.ProductId);
    }
}

/*
var variants = await _variantRepository
       .GetManyAsync(product.ProductVariantIds, cancellationToken);
   
foreach (var variant in variants)
{
   var images = await _variantImageRepository
       .GetManyAsync(variant.ImageIds, cancellationToken);
   
   imagesToVariants.Add(variant.Id, images);
}
*/