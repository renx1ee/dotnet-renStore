namespace RenStore.Catalog.Domain.Aggregates.VariantAttributes.Events;

public record VariantAttributeValueUpdated(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid AttributeId,
    string Value);