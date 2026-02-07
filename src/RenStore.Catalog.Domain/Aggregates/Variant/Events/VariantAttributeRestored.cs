namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record VariantAttributeRestored(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid AttributeId);