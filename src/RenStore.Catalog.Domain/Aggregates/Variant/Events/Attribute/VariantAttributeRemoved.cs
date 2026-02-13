namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Attribute;

public record VariantAttributeRemoved(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid AttributeId);