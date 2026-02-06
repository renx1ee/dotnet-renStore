namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record VariantAttributeRemoved(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid AttributeId);