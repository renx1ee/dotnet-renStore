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
    public static readonly Dictionary<string, Type> DomainEventsNameToType = new()
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
        { "product-approved",                                  typeof(ProductApprovedEvent) },
        { "product-archived",                                  typeof(ProductArchivedEvent) },
        { "product-created",                                   typeof(ProductCreatedEvent) },
        { "product-hidden",                                    typeof(ProductHiddenEvent) },
        { "product-moved-to-draft",                            typeof(ProductMovedToDraftEvent) },
        { "product-published",                                 typeof(ProductPublishedEvent) },
        { "product-rejected",                                  typeof(ProductRejectedEvent) },
        { "product-removed",                                   typeof(ProductRemovedEvent) },
        { "product-restored",                                  typeof(ProductRestoredEvent) },
        { "product-variant-reference-created",                 typeof(ProductVariantReferenceCreatedEvent) },
        { "product-variant-reference-removed",                 typeof(ProductVariantReferenceRemovedEvent) },
        
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
    
    public static readonly Dictionary<Type, string> DomainEventsTypeToName = new()
    {
        // Details
        { typeof(VariantDetailsCaringOfThingsUpdated),         "variant-details-caring-of-things-updated" },
        { typeof(VariantDetailsCompositionUpdated),            "variant-details-composition-updated" },
        { typeof(VariantDetailsCountryOfManufactureIdUpdated), "variant-details-country-of-manufacture-id-updated" },
        { typeof(VariantDetailsCreated),                       "variant-details-created" },
        { typeof(VariantDetailsDecorativeElementsUpdated),     "variant-details-decorative-elements-updated" },
        { typeof(VariantDetailsDescriptionUpdated),            "variant-details-description-updated" },
        { typeof(VariantDetailsEquipmentUpdated),              "variant-details-equipment-updated" },
        { typeof(VariantDetailsModelFeaturesUpdated),          "variant-details-model-features-updated" },
        { typeof(VariantDetailsTypeOfPackingUpdated),          "variant-details-type-of-packing-updated" },
        
        // Attribute
        { typeof(AttributeCreated),                            "attribute-created" },
        { typeof(AttributeKeyUpdated),                         "attribute-key-updated" },
        { typeof(AttributeRemoved),                            "attribute-removed" },
        { typeof(AttributeRestored),                           "attribute-restored" },
        { typeof(AttributeValueUpdated),                       "attribute-value-updated" },
        
        // Media
        { typeof(ImageCreated),                                "image-created" },
        { typeof(ImageDimensionUpdated),                       "image-dimension-updated" },
        { typeof(ImageFileSizeBytesUpdated),                   "image-file-size-byte-updated" },
        { typeof(ImageMainSet),                                "image-main-set" },
        { typeof(ImageMainUnset),                              "image-main-unset" },
        { typeof(ImageRemoved),                                "image-removed" },
        { typeof(ImageRestored),                               "image-restored" },
        { typeof(ImageSortOrderUpdated),                       "image-sort-order-updated" },
        { typeof(ImageStoragePathUpdated),                     "image-storage-path-updated" },
        
        // Product
        { typeof(ProductApprovedEvent),                             "product-approved" },
        { typeof(ProductArchivedEvent),                             "product-archived" },
        { typeof(ProductCreatedEvent),                              "product-created" },
        { typeof(ProductHiddenEvent),                               "product-hidden" },
        { typeof(ProductMovedToDraftEvent),                         "product-moved-to-draft" },
        { typeof(ProductPublishedEvent),                            "product-published" },
        { typeof(ProductRejectedEvent),                             "product-rejected" },
        { typeof(ProductRemovedEvent),                              "product-removed" },
        { typeof(ProductRestoredEvent),                             "product-restored" },
        { typeof(ProductVariantReferenceCreatedEvent),              "product-variant-reference-created" },
        { typeof(ProductVariantReferenceRemovedEvent),              "product-variant-reference-removed" },
        
        // Variant
        { typeof(MainImageIdSet),                              "variant-main-image-id-set" },
        { typeof(PriceCreated),                                "variant-price-created" },
        { typeof(VariantSizeCreated),                          "variant-size-created" },
        { typeof(VariantSizeRemoved),                          "variant-size-removed" },
        { typeof(VariantSizeRestored),                         "variant-size-restored" },
        { typeof(VariantArchived),                             "variant-archived" },
        { typeof(VariantCreated),                              "variant-created" },
        { typeof(VariantDrafted),                              "variant-drafted" },
        { typeof(VariantNameUpdated),                          "variant-name-updated" },
        { typeof(VariantPublished),                            "variant-published" },
        { typeof(VariantRemoved),                              "variant-removed" },
    };
}