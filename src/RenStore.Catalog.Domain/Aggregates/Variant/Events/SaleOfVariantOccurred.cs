namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record SaleOfVariantOccurred(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    int Count);