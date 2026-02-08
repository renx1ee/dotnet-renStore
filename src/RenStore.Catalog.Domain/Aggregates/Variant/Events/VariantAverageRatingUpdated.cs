namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

/// <summary>
/// Records an update to the average customer rating of a product variant.
/// Reflects changes based on new customer reviews or recalculations.
/// </summary>
/// <param name="VariantId">Identifier of the rated product variant</param>
/// <param name="OccurredAt">Timestamp when the rating was updated</param>
/// <param name="Score">New average rating score (typically 0.0 to 5.0)</param>
/// <remarks>
/// Used for ranking algorithms, quality metrics, and customer decision-making.
/// Scores are usually calculated from multiple reviews and updated periodically.
/// </remarks>
public record VariantAverageRatingUpdated(
    Guid VariantId,
    DateTimeOffset OccurredAt,
    decimal Score);