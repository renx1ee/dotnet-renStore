namespace RenStore.Catalog.Domain.Aggregates.VariantMedia.Events;

public record ImageSortOrderUpdated(
    DateTimeOffset OccurredAt,
    Guid ImageId,
    short SortOrder);