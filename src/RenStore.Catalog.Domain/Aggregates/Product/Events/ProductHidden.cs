using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Product.Events;

/// <summary>
/// The event occurred when the product has been hidden.
/// </summary>
/// <param name="ProductId">Unique product ID.</param>
/// <param name="OccurredAt">Time of occurrence of the event.</param>
public record ProductHidden(
    Guid EventId,
    Guid ProductId,
    DateTimeOffset OccurredAt)
    : IDomainEvent;