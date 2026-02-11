namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public record VariantDetailsCompositionUpdated(
    DateTimeOffset OccurredAt,
    string Composition,
    Guid VariantId);