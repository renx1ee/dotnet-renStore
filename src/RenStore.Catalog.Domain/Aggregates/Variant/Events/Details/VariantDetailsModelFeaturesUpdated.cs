namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public record VariantDetailsModelFeaturesUpdated(
    DateTimeOffset OccurredAt,
    string ModelFeatures,
    Guid VariantId);