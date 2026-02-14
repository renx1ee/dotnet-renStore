namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsModelFeaturesUpdated(
    DateTimeOffset OccurredAt,
    string ModelFeatures,
    Guid VariantId);