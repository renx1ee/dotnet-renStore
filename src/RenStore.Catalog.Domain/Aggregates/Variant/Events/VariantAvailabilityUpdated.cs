namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

/// <summary>
/// Records a change in the availability status of a product variant.
/// Tracks when a variant becomes available or unavailable for customer purchase.
/// </summary>
/// <param name="OccurredAt">Timestamp when the availability changed</param>
/// <param name="VariantId">Identifier of the product variant</param>
/// <param name="IsAvailable">New availability status (true = available for purchase)</param>
/// <remarks>
/// Availability can change due to stock levels, manual intervention, or business rules.
/// This event triggers catalog visibility updates and inventory notifications.
/// </remarks>
public record VariantAvailabilityUpdated(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    bool IsAvailable);