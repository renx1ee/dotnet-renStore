using RenStore.Catalog.Domain.Aggregates.Attribute.Events;
using RenStore.Catalog.Domain.Aggregates.Media.Events;
using RenStore.Catalog.Domain.Aggregates.Product.Events;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Images;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Price;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Size;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;
using RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

namespace RenStore.Catalog.Persistence.EventStore;

public static class DomainEventMappings
{
    public static readonly Dictionary<string, Type> DomainEvents = new()
    {
        // Details
        { "variant-details-caring-of-things-updated",          typeof(VariantDetailsCaringOfThingsUpdated) },
        { "variant-details-composition-updated",               typeof(VariantDetailsCompositionUpdated) },
        { "variant-details-country-of-manufacture-id-updated", typeof(VariantDetailsCountryOfManufactureIdUpdated) },
        { "variant-details-created",                           typeof(VariantDetailsCreated) },
        { "variant-details-decorative-elements-updated",       typeof(VariantDetailsDecorativeElementsUpdated) },
        { "variant-details-description-updated",               typeof(VariantDetailsDescriptionUpdated) },
        { "variant-details-equipment-updated",                 typeof(VariantDetailsEquipmentUpdated) },
        { "variant-details-model-features-updated",            typeof(VariantDetailsModelFeaturesUpdated) },
        { "variant-details-type-of-packing-updated",           typeof(VariantDetailsTypeOfPackingUpdated) },
        
        // Attribute
        { "attribute-created",                                 typeof(AttributeCreated) },
        { "attribute-key-updated",                             typeof(AttributeKeyUpdated) },
        { "attribute-removed",                                 typeof(AttributeRemoved) },
        { "attribute-restored",                                typeof(AttributeRestored) },
        { "attribute-value-updated",                           typeof(AttributeValueUpdated) },
        
        // Media
        { "image-created",                                     typeof(ImageCreated) },
        { "image-dimension-updated",                           typeof(ImageDimensionUpdated) },
        { "image-file-size-byte-updated",                      typeof(ImageFileSizeBytesUpdated) },
        { "image-main-set",                                    typeof(ImageMainSet) },
        { "image-main-unset",                                  typeof(ImageMainUnset) },
        { "image-removed",                                     typeof(ImageRemoved) },
        { "image-restored",                                    typeof(ImageRestored) },
        { "image-sort-order-updated",                          typeof(ImageSortOrderUpdated) },
        { "image-storage-path-updated",                        typeof(ImageStoragePathUpdated) },
        
        // Product
        { "product-approved",                                  typeof(ProductApproved) },
        { "product-archived",                                  typeof(ProductArchived) },
        { "product-created",                                   typeof(ProductCreated) },
        { "product-hidden",                                    typeof(ProductHidden) },
        { "product-moved-to-draft",                            typeof(ProductMovedToDraft) },
        { "product-published",                                 typeof(ProductPublished) },
        { "product-rejected",                                  typeof(ProductRejected) },
        { "product-removed",                                   typeof(ProductRemoved) },
        { "product-restored",                                  typeof(ProductRestored) },
        { "product-variant-reference-created",                 typeof(ProductVariantReferenceCreated) },
        { "product-variant-reference-removed",                 typeof(ProductVariantReferenceRemoved) },
        
        // Variant
        { "variant-main-image-id-set",                         typeof(MainImageIdSet) },
        { "variant-price-created",                             typeof(PriceCreated) },
        { "variant-size-created",                              typeof(VariantSizeCreated) },
        { "variant-size-removed",                              typeof(VariantSizeRemoved) },
        { "variant-size-restored",                             typeof(VariantSizeRestored) },
        { "variant-archived",                                  typeof(VariantArchived) },
        { "variant-created",                                   typeof(VariantCreated) },
        { "variant-drafted",                                   typeof(VariantDrafted) },
        { "variant-name-updated",                              typeof(VariantNameUpdated) },
        { "variant-published",                                 typeof(VariantPublished) },
        { "variant-removed",                                   typeof(VariantRemoved) },
    };
}