namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsCaringOfThingsUpdated(
    DateTimeOffset OccurredAt,
    string CaringOfThings,
    Guid VariantId);