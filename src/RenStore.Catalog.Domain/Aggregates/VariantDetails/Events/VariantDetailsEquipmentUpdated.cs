namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsEquipmentUpdated(
    DateTimeOffset OccurredAt,
    string Equipment,
    Guid VariantId);