namespace RenStore.Catalog.Domain.Aggregates.Attribute.Events;

public record AttributeValueUpdated(
    DateTimeOffset OccurredAt,
    Guid AttributeId,
    string Value);