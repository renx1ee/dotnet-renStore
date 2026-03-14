using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Attribute.Events;

/// <summary>
/// Records the restoration of a previously removed attribute to a product variant.
/// Reinstates attribute information that was temporarily unavailable or incorrectly deleted.
/// </summary>
/// <param name="OccurredAt">Timestamp when the attribute was restored</param>
/// <param name="AttributeId">Identifier of the specific attribute being restored</param>
/// <remarks>
/// Restoration preserves the attribute's original data and business context.
/// Used in scenarios like data correction, seasonal availability, or supplier changes.
/// </remarks>
public sealed record AttributeRestoredEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid AttributeId) 
    : IDomainEvent;