namespace RenStore.Catalog.Domain.Aggregates.VariantAttributes.Events;

/// <summary>
/// Records the removal of an attribute from a product variant.
/// Used when attribute information becomes outdated, incorrect, or no longer applicable.
/// </summary>
/// <param name="OccurredAt">Timestamp when the attribute was removed</param>
/// <param name="VariantId">Identifier of the product variant losing the attribute</param>
/// <param name="AttributeId">Identifier of the specific attribute being removed</param>
/// <remarks>
/// Removal is typically a soft delete for audit trail preservation.
/// Historical attributes may still be referenced in past orders or analytics.
/// </remarks>
public record AttributeRemoved(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid AttributeId);