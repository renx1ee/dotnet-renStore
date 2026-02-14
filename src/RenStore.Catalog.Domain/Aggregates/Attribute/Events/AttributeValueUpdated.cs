namespace RenStore.Catalog.Domain.Aggregates.Attribute.Events;

public record AttributeValueUpdated(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid AttributeId,
    string Value);