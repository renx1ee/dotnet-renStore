namespace RenStore.Catalog.Domain.Aggregates.Attribute.Events;

public record AttributeKeyUpdated(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid AttributeId,
    string Key);