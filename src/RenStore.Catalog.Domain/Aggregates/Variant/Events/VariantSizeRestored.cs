namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record VariantSizeRestored(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid VariantSizeId);