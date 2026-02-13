namespace RenStore.Catalog.Domain.Aggregates.Media.Events;

public record ImageSortOrderUpdated(
    DateTimeOffset OccurredAt,
    Guid ImageId,
    short SortOrder);