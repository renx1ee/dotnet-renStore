using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

/// <summary>
/// Records a reduction in available inventory for a product variant.
/// Used for order fulfillment, inventory adjustments, or returns processing.
/// </summary>
/// <param name="OccurredAt">Timestamp when stock was reduced</param>
/// <param name="VariantId">Identifier of the product variant</param>
/// <param name="Count">Number of units removed from inventory</param>
/// <remarks>
/// Stock reductions are validated against current inventory levels.
/// When inventory reaches zero, the variant typically becomes unavailable for purchase.
/// Multiple events may represent a single transaction involving multiple units.
/// </remarks>
public record VariantStockWrittenOff(
    DateTimeOffset OccurredAt,
    WriteOffReason Reason,
    Guid VariantId,
    int Count);