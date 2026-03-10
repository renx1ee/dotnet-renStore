using RenStore.Catalog.Domain.Aggregates.Media;
using RenStore.Catalog.Domain.Aggregates.Product;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Aggregates.VariantDetails;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.DomainService;

public class VariantPublishService : IVariantPublishService
{
    private const int MaxImagesCount     = 50;
    private const int MaxAttributesCount = 50;
    
    public void Publish(
        DateTimeOffset now, 
        Product product, 
        IReadOnlyCollection<ProductVariant> variants, 
        IReadOnlyCollection<Attribute> attributes,
        IReadOnlyCollection<VariantImage> images, 
        VariantDetail detail)
    {
        if (product == null)
            throw new DomainException(
                "Product cannot be null.");
        
        if(!images.Any())
            throw new DomainException(
                "Variant image must contains at least one item.");
        
        if (detail == null)
            throw new DomainException(
                "Variant must have details.");
        
        if (!variants.Any())
            throw new DomainException(
                "Product variant must contains at least one size.");

        MaxAttributesCountValidation(attributes.Count);
        MaxImagesCountValidation(images.Count);

        foreach (var variant in variants)
        {
            if(variant.Status == ProductVariantStatus.Deleted)
                throw new DomainException(
                    "Product variant already deleted.");
            
            foreach (var size in variant.Sizes)
            {
                if(size.IsDeleted)
                    throw new DomainException(
                        "Product variant size already deleted.");

                var activePrice = size.Prices
                    .FirstOrDefault(x => x.IsActive);

                if (activePrice is null)
                    throw new DomainException($"Size: {size.Id} has no active price.");

                if (activePrice.Price.Amount <= 0)
                    throw new DomainException($"Size: {size.Id} has invalid price.");
            }

            /*if (images.Any(x => x.VariantId) != variant.Id)
            {
                
            }
            */

            foreach (var image in images)
            {
                
            }
        }
        
        
    }

    private static void MaxImagesCountValidation(int count)
    {
        if (count >= MaxImagesCount)
            throw new DomainException($"Product images count must be less then {MaxImagesCount}.");
    }
    
    private static void MaxAttributesCountValidation(int count)
    {
        if (count >= MaxAttributesCount)
            throw new DomainException($"Attributes count must be less then {MaxAttributesCount}.");
    }
    
    // сделать установку MainImageId в Variant
    //  if (_attributeIds.Count >= MAX_ATTRIBUTES)
    //  ProductAttributeRules.MaxAttributesCountValidation(_attributes.Count);
}