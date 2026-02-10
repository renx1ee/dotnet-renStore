namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record ProductVariantRemoved(
    Guid VariantId,
    DateTimeOffset OccurredAt);