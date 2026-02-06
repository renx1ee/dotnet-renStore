namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record VariantAverageRatingUpdated(
    Guid VariantId,
    DateTimeOffset OccurredAt,
    decimal Score);