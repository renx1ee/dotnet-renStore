namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public record VariantDetailsCaringOfThingsUpdated(
    DateTimeOffset OccurredAt,
    string CaringOfThings,
    Guid VariantId);