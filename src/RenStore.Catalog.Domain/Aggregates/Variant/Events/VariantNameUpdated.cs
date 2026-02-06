namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record VariantNameUpdated(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    string Name);