namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Image;

public record VariantImageSortOrderUpdated(
    DateTimeOffset OccurredAt,
    Guid ImageId,
    short SortOrder);