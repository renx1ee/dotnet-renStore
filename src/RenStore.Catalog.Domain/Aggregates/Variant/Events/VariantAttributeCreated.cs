namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record VariantAttributeCreated(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    string Key,
    string Value);