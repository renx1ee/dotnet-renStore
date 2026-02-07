namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record VariantSizeRemoved(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid VariantSizeId);