namespace RenStore.Catalog.Domain.Aggregates.VariantAttributes.Events;

/// <summary>
/// Records the restoration of a previously removed attribute to a product variant.
/// Reinstates attribute information that was temporarily unavailable or incorrectly deleted.
/// </summary>
/// <param name="OccurredAt">Timestamp when the attribute was restored</param>
/// <param name="VariantId">Identifier of the product variant receiving the restored attribute</param>
/// <param name="AttributeId">Identifier of the specific attribute being restored</param>
/// <remarks>
/// Restoration preserves the attribute's original data and business context.
/// Used in scenarios like data correction, seasonal availability, or supplier changes.
/// </remarks>
public record AttributeRestored(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid AttributeId);