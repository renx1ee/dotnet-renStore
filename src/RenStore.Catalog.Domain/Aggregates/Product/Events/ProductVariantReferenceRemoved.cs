using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Product.Events;

/// <summary>
/// The event occurred when the product has been removed reference with product variant.
/// </summary>
/// <param name="ProductId">Unique product ID.</param>
/// <param name="VariantId">Unique product variant ID.</param>
/// <param name="OccurredAt">Time of occurrence of the event.</param>
public record ProductVariantReferenceRemoved(
    Guid EventId,
    Guid ProductId,
    Guid VariantId,
    DateTimeOffset OccurredAt)
    : IDomainEvent;