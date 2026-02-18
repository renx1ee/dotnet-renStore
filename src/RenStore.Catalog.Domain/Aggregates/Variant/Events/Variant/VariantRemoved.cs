namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;

public record VariantRemoved(
    Guid VariantId,
    DateTimeOffset OccurredAt);