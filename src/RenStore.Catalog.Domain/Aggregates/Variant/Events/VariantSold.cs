namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

/// <summary>
/// Represents a business transaction where customers purchased units of this product variant.
/// Used for sales analytics, inventory forecasting, and popularity tracking.
/// </summary>
/// <param name="OccurredAt">Timestamp when the sale was recorded</param>
/// <param name="VariantId">Identifier of the purchased product variant</param>
/// <param name="Count">Number of units sold in this transaction</param>
/// <remarks>
/// This domain event is raised after successful order fulfillment.
/// Multiple events may be raised for a single order containing multiple variants.
/// </remarks>
public record VariantSold(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    int Count);