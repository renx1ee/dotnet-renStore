using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Product.Events;

/// <summary>
/// The event occurred when the product has been removed.
/// </summary>
/// <param name="ProductId">Unique product ID.</param>
/// <param name="OccurredAt">Time of occurrence of the event.</param>
public record ProductRemovedEvent(
    Guid UpdatedById,
    string UpdatedByRole,
    Guid EventId,
    Guid ProductId, 
    ProductStatus Status,
    DateTimeOffset OccurredAt)
    : IDomainEvent;