namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Size;

public record VariantSizeRemovedFromStock(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid VariantSizeId,
    int Count);