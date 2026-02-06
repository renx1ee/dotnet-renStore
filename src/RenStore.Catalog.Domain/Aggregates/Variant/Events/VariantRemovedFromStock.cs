namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record VariantRemovedFromStock(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    int Count);