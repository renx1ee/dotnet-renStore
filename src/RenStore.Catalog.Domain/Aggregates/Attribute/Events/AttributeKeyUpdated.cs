namespace RenStore.Catalog.Domain.Aggregates.VariantAttributes.Events;

public record AttributeKeyUpdated(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid AttributeId,
    string Key);