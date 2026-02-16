namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Size;

/// <summary>
/// Records the removal of a size option from a product variant's available selections.
/// Used when a size is discontinued or permanently unavailable.
/// </summary>
/// <param name="OccurredAt">Timestamp when the size was removed</param>
/// <param name="VariantId">Identifier of the product variant</param>
/// <param name="SizeId">Identifier of the removed size option</param>
/// <remarks>
/// Size removal affects:
/// - Current purchase options for customers
/// - Inventory management for that specific size
/// - Historical order references
/// 
/// Typically implements soft deletion to preserve order history and analytics.
/// Consider impact on existing customer preferences and search filters.
/// </remarks>
public record VariantSizeRemoved(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid SizeId);