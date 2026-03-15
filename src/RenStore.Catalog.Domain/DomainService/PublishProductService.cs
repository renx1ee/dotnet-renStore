using RenStore.Catalog.Domain.Aggregates.Media;
using RenStore.Catalog.Domain.Aggregates.Product;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Constants;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.DomainService;

public sealed class PublishProductService : IPublishProductService
{
    public void Publish(
        DateTimeOffset now, 
        Product product, 
        IReadOnlyCollection<ProductVariant> variants,
        IReadOnlyDictionary<Guid, IReadOnlyCollection<VariantImage>> imagesByVariants)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentNullException.ThrowIfNull(variants);
        ArgumentNullException.ThrowIfNull(imagesByVariants);

        var variantsList = variants as IList<ProductVariant> ?? variants.ToList();
        
        var errors = new List<string>();
        
        if (!variantsList.Any())
        {
            throw new DomainException(
                "Product variant must have at least one variant.");
        }

        if (variantsList.Count > CatalogConstants.Product.MaxVariantsCount)
        {
            throw new DomainException(
                $"Product must have max {CatalogConstants.Product.MaxVariantsCount} variants.");
        }
        
        if (!imagesByVariants.Any())
        {
            throw new DomainException(
                "Variant image must contains at least one item.");
        }

        foreach (var variant in variants)
        {
            if (variant.Status == ProductVariantStatus.Published)
                continue;
            
            if (variant.ProductId != product.Id)
            {
                errors.Add("Variant does not contain this product ID.");
                continue;
            }
            
            var images = imagesByVariants
                .GetValueOrDefault(variant.Id, []);

            if (!images.Any())
                errors.Add($"Variant {variant.Id} must have at least one image");
            
            if(images.Count > CatalogConstants.Image.MaxImagesCount)
                errors.Add($"Images must have max {CatalogConstants.Image.MaxImagesCount} images count.");
            
            try
            {
                variant.Publish(now);
            }
            catch (DomainException e)
            {
                errors.Add($"Variant {variant.Id}: {e.Message}");
            }
        }
        
        if (errors.Any())
            throw new DomainException(string.Join("\n", errors));
            
        product.MarkAsPublished(now);
    }
}