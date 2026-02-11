namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Size;

/// <summary>
/// Records the restoration of a previously removed size option to a product variant.
/// Reinstates a size that was temporarily unavailable or incorrectly removed.
/// </summary>
/// <param name="OccurredAt">Timestamp when the size was restored</param>
/// <param name="VariantId">Identifier of the product variant</param>
/// <param name="VariantSizeId">Identifier of the restored size option</param>
/// <remarks>
/// Used for scenarios like:
/// - Supplier restocking discontinued sizes
/// - Seasonal size availability
/// - Correction of mistaken removals
/// Restored sizes retain their original configuration and historical data.
/// Customers can once again select this size in new orders.
/// </remarks>
public record VariantSizeRestored(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid VariantSizeId);