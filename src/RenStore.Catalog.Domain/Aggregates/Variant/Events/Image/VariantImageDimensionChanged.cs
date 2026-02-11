namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Image;

public record VariantImageDimensionChanged(
    DateTimeOffset OccurredAt,
    Guid ImageId,
    int Weight,
    int Height);