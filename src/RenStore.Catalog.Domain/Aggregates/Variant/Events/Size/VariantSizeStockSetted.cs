namespace RenStore.Catalog.Domain.Aggregates.VariantSizes.Events;

public record VariantSizeStockSetted(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid VariantSizeId,
    int NewStock);