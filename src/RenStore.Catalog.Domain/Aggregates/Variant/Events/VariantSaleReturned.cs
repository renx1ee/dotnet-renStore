namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record VariantSaleReturned(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    int Count);