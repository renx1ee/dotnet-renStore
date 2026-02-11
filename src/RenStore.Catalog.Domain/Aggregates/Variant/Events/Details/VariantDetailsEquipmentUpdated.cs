namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public record VariantDetailsEquipmentUpdated(
    DateTimeOffset OccurredAt,
    string Equipment,
    Guid VariantId);