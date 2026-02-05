namespace RenStore.Catalog.Domain.Aggregates.Product.Events;

/// <summary>
/// The event occurred when the product rating has been updated.
/// </summary>
/// <param name="ProductId">Unique product ID.</param>
/// <param name="OccurredAt">Time of occurrence of the event.</param>
/// <param name="Score">Rating value assigned to the product.</param>
public record ProductAverageRatingUpdated(
    Guid ProductId,
    DateTimeOffset OccurredAt,
    decimal Score);