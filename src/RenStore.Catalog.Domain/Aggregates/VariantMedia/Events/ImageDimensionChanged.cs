namespace RenStore.Catalog.Domain.Aggregates.VariantMedia.Events;

public record ImageDimensionChanged(
    DateTimeOffset OccurredAt,
    Guid ImageId,
    int Weight,
    int Height);