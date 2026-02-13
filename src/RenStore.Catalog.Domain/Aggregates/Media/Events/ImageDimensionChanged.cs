namespace RenStore.Catalog.Domain.Aggregates.Media.Events;

public record ImageDimensionChanged(
    DateTimeOffset OccurredAt,
    Guid ImageId,
    int Weight,
    int Height);