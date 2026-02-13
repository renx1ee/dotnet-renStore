namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Attribute;

public record VariantAttributeAdded(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid AttributeId);