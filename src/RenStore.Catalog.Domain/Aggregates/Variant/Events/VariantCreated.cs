namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record VariantCreated(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid ProductId,
    int ColorId,
    string Name,
    int InStock,
    string Url);