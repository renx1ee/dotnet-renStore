namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record VariantStockAdded(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    int Count);