using RenStore.Catalog.Domain.Aggregates.Category.Events;
using RenStore.Catalog.Domain.Aggregates.Media.Events;
using RenStore.Catalog.Domain.Aggregates.Product.Events;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Attribute;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Images;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Price;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Size;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;

namespace RenStore.Catalog.Persistence.EventStore;

public static class DomainEventMappings
{
    public static readonly Dictionary<string, Type> DomainEventsNameToType = new()
    {
        // Details
        { "variant-details-caring-of-things-updated",          typeof(VariantDetailsCaringOfThingsUpdatedEvent) },
        { "variant-details-composition-updated",               typeof(VariantDetailsCompositionUpdatedEvent) },
        { "variant-details-country-of-manufacture-id-updated", typeof(VariantDetailsCountryOfManufactureIdUpdatedEvent) },
        { "variant-details-created",                           typeof(VariantDetailsCreatedEvent) },
        { "variant-details-decorative-elements-updated",       typeof(VariantDetailsDecorativeElementsUpdatedEvent) },
        { "variant-details-description-updated",               typeof(VariantDetailsDescriptionUpdatedEvent) },
        { "variant-details-equipment-updated",                 typeof(VariantDetailsEquipmentUpdatedEvent) },
        { "variant-details-model-features-updated",            typeof(VariantDetailsModelFeaturesUpdatedEvent) },
        { "variant-details-type-of-packing-updated",           typeof(VariantDetailsTypeOfPackingUpdatedEvent) },
        
        // Attribute
        { "attribute-created",                                 typeof(AttributeCreatedEvent) },
        { "attribute-key-updated",                             typeof(AttributeKeyUpdatedEvent) },
        { "attribute-removed",                                 typeof(AttributeRemovedEvent) },
        { "attribute-restored",                                typeof(AttributeRestoredEvent) },
        { "attribute-value-updated",                           typeof(AttributeValueUpdatedEvent) },
        
        // Media
        { "image-created",                                     typeof(ImageCreatedEvent) },
        { "image-dimension-updated",                           typeof(ImageDimensionUpdatedEvent) },
        { "image-file-size-byte-updated",                      typeof(ImageFileSizeBytesUpdatedEvent) },
        { "image-main-set",                                    typeof(ImageMainSetEvent) },
        { "image-main-unset",                                  typeof(ImageMainUnsetEvent) },
        { "image-removed",                                     typeof(VariantImageRemovedEvent) },
        { "image-restored",                                    typeof(ImageRestoredEvent) },
        { "image-sort-order-updated",                          typeof(ImageSortOrderUpdatedEvent) },
        { "image-storage-path-updated",                        typeof(ImageStoragePathUpdatedEvent) },
        
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
        { "variant-main-image-id-set",                         typeof(MainImageIdSetEvent) },
        { "variant-price-created",                             typeof(PriceCreatedEvent) },
        { "variant-size-created",                              typeof(VariantSizeCreatedEvent) },
        { "variant-size-removed",                              typeof(VariantSizeRemovedEvent) },
        { "variant-size-restored",                             typeof(VariantSizeRestoredEvent) },
        { "variant-archived",                                  typeof(VariantArchivedEvent) },
        { "variant-created",                                   typeof(VariantCreatedEvent) },
        { "variant-drafted",                                   typeof(VariantDraftedEvent) },
        { "variant-name-updated",                              typeof(VariantNameUpdatedEvent) },
        { "variant-published",                                 typeof(VariantPublishedEvent) },
        { "variant-removed",                                   typeof(VariantRemovedEvent) },
        { "variant-image-reference-added",                     typeof(AddedImageReferenceEvent) },
        { "variant-image-reference-removed",                   typeof(RemoveImageReferenceEvent) },
        
        // Category
        { "category-activated",                                typeof(CategoryActivatedEvent) },
        { "category-created",                                  typeof(CategoryCreatedEvent) },
        { "category-deactivated",                              typeof(CategoryDeactivatedEvent) },
        { "category-deleted",                                  typeof(CategoryDeletedEvent) },
        { "category-description-changed",                      typeof(CategoryDescriptionChangedEvent) },
        { "category-name-changed",                             typeof(CategoryNameChangedEvent) },
        { "category-name-ru-changed",                          typeof(CategoryNameRuChangedEvent) },
        { "category-restored",                                 typeof(CategoryRestoredEvent) },
        { "sub-category-activated",                            typeof(SubCategoryActivatedEvent) },
        { "sub-category-created",                              typeof(SubCategoryCreatedEvent) },
        { "sub-category-deactivated",                          typeof(SubCategoryDeactivatedEvent) },
        { "sub-category-deleted",                              typeof(SubCategoryDeletedEvent) },
        { "sub-category-description-changed",                  typeof(SubCategoryDescriptionChangedEvent) },
        { "sub-category-name-changed",                         typeof(SubCategoryNameChangedEvent) },
        { "sub-category-name-ru-changed",                      typeof(SubCategoryNameRuChangedEvent) },
        { "sub-category-restored",                             typeof(SubCategoryRestoredEvent) },
    };
    
    public static readonly Dictionary<Type, string> DomainEventsTypeToName = new()
    {
        // Details
        { typeof(VariantDetailsCaringOfThingsUpdatedEvent),         "variant-details-caring-of-things-updated" },
        { typeof(VariantDetailsCompositionUpdatedEvent),            "variant-details-composition-updated" },
        { typeof(VariantDetailsCountryOfManufactureIdUpdatedEvent), "variant-details-country-of-manufacture-id-updated" },
        { typeof(VariantDetailsCreatedEvent),                       "variant-details-created" },
        { typeof(VariantDetailsDecorativeElementsUpdatedEvent),     "variant-details-decorative-elements-updated" },
        { typeof(VariantDetailsDescriptionUpdatedEvent),            "variant-details-description-updated" },
        { typeof(VariantDetailsEquipmentUpdatedEvent),              "variant-details-equipment-updated" },
        { typeof(VariantDetailsModelFeaturesUpdatedEvent),          "variant-details-model-features-updated" },
        { typeof(VariantDetailsTypeOfPackingUpdatedEvent),          "variant-details-type-of-packing-updated" },
        
        // Attribute
        { typeof(AttributeCreatedEvent),                       "attribute-created" },
        { typeof(AttributeKeyUpdatedEvent),                    "attribute-key-updated" },
        { typeof(AttributeRemovedEvent),                       "attribute-removed" },
        { typeof(AttributeRestoredEvent),                      "attribute-restored" },
        { typeof(AttributeValueUpdatedEvent),                  "attribute-value-updated" },
        
        // Media
        { typeof(ImageCreatedEvent),                           "image-created" },
        { typeof(ImageDimensionUpdatedEvent),                  "image-dimension-updated" },
        { typeof(ImageFileSizeBytesUpdatedEvent),              "image-file-size-byte-updated" },
        { typeof(ImageMainSetEvent),                           "image-main-set" },
        { typeof(ImageMainUnsetEvent),                         "image-main-unset" },
        { typeof(VariantImageRemovedEvent),                           "image-removed" },
        { typeof(ImageRestoredEvent),                          "image-restored" },
        { typeof(ImageSortOrderUpdatedEvent),                  "image-sort-order-updated" },
        { typeof(ImageStoragePathUpdatedEvent),                "image-storage-path-updated" },
        
        // Product
        { typeof(ProductApprovedEvent),                        "product-approved" },
        { typeof(ProductArchivedEvent),                        "product-archived" },
        { typeof(ProductCreatedEvent),                         "product-created" },
        { typeof(ProductHiddenEvent),                          "product-hidden" },
        { typeof(ProductMovedToDraftEvent),                    "product-moved-to-draft" },
        { typeof(ProductPublishedEvent),                       "product-published" },
        { typeof(ProductRejectedEvent),                        "product-rejected" },
        { typeof(ProductRemovedEvent),                         "product-removed" },
        { typeof(ProductRestoredEvent),                        "product-restored" },
        { typeof(ProductVariantReferenceCreatedEvent),         "product-variant-reference-created" },
        { typeof(ProductVariantReferenceRemovedEvent),         "product-variant-reference-removed" },
        
        // Variant
        { typeof(MainImageIdSetEvent),                         "variant-main-image-id-set" },
        { typeof(PriceCreatedEvent),                           "variant-price-created" },
        { typeof(VariantSizeCreatedEvent),                     "variant-size-created" },
        { typeof(VariantSizeRemovedEvent),                     "variant-size-removed" },
        { typeof(VariantSizeRestoredEvent),                    "variant-size-restored" },
        { typeof(VariantArchivedEvent),                        "variant-archived" },
        { typeof(VariantCreatedEvent),                         "variant-created" },
        { typeof(VariantDraftedEvent),                         "variant-drafted" },
        { typeof(VariantNameUpdatedEvent),                     "variant-name-updated" },
        { typeof(VariantPublishedEvent),                       "variant-published" },
        { typeof(VariantRemovedEvent),                         "variant-removed" },
        { typeof(AddedImageReferenceEvent),                    "variant-image-reference-added" },
        { typeof(RemoveImageReferenceEvent),                   "variant-image-reference-removed" },
        
        // Category
        { typeof(CategoryActivatedEvent),                      "category-activated" },
        { typeof(CategoryCreatedEvent),                        "category-created" },
        { typeof(CategoryDeactivatedEvent),                    "category-deactivated" },
        { typeof(CategoryDeletedEvent),                        "category-deleted" },
        { typeof(CategoryDescriptionChangedEvent),             "category-description-changed" },
        { typeof(CategoryNameChangedEvent),                    "category-name-changed" },
        { typeof(CategoryNameRuChangedEvent),                  "category-name-ru-changed" },
        { typeof(CategoryRestoredEvent),                       "category-restored" },
        { typeof(SubCategoryActivatedEvent),                   "sub-category-activated" },
        { typeof(SubCategoryCreatedEvent),                     "sub-category-created" },
        { typeof(SubCategoryDeactivatedEvent),                 "sub-category-deactivated" },
        { typeof(SubCategoryDeletedEvent),                     "sub-category-deleted" },
        { typeof(SubCategoryDescriptionChangedEvent),          "sub-category-description-changed" },
        { typeof(SubCategoryNameChangedEvent),                 "sub-category-name-changed" },
        { typeof(SubCategoryNameRuChangedEvent),               "sub-category-name-ru-changed" },
        { typeof(SubCategoryRestoredEvent),                    "sub-category-restored" },
    };
    
    /// <summary>
    /// Resolves the event type name for serialization.
    /// Throws if the event type is not registered — fail fast, no silent data loss.
    /// </summary>
    public static string GetEventName(Type eventType)
    {
        if (DomainEventsTypeToName.TryGetValue(eventType, out var name))
            return name;

        throw new InvalidOperationException(
            $"Domain event type '{eventType.FullName}' is not registered in {nameof(DomainEventMappings)}.");
    }
    
    /// <summary>
    /// Resolves the CLR type from a persisted event name.
    /// Throws if the name is unknown — prevents silent deserialization gaps.
    /// </summary>
    public static Type GetEventType(string eventName)
    {
        if (DomainEventsNameToType.TryGetValue(eventName, out var type))
            return type;
        
        throw new InvalidOperationException(
            $"Unknown domain event name '{eventName}'. Register it in {nameof(DomainEventMappings)}.");
    }
}