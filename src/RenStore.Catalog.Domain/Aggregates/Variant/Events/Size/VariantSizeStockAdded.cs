namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Size;

public record VariantSizeStockAdded(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid VariantSizeId,
    int Count);