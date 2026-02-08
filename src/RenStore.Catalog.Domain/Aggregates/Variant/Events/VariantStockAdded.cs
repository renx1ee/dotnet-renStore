namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

/// <summary>
/// Records an increase in available inventory for a product variant.
/// Used for restocking from suppliers, returns processing, or inventory corrections.
/// </summary>
/// <param name="OccurredAt">Timestamp when stock was added</param>
/// <param name="VariantId">Identifier of the product variant</param>
/// <param name="Count">Number of units added to inventory</param>
/// <remarks>
/// Stock additions:
/// - May automatically re-enable variant availability if stock was zero
/// - Are cumulative and can be applied multiple times
/// - Should be paired with corresponding purchase or production events
/// 
/// Positive stock changes improve customer purchase success rates.
/// </remarks>
public record VariantStockAdded(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    int Count);