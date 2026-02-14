namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsCompositionUpdated(
    DateTimeOffset OccurredAt,
    string Composition,
    Guid VariantId);