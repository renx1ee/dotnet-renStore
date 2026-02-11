namespace RenStore.Catalog.Domain.Aggregates.VariantAttributes.Events;

public record VariantAttributeKeyUpdated(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid AttributeId,
    string Key);