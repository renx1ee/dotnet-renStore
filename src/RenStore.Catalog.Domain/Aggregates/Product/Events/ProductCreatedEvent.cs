using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Product.Events;

/// <summary>
/// The event occurred when the product has been created.
/// </summary>
/// <param name="ProductId">Unique product ID.</param>
/// <param name="SellerId">Unique seller ID.</param>
/// <param name="SubCategoryId">Unique sub category ID.</param>
/// <param name="Status">The product at the time of creation.</param>
/// <param name="OccurredAt">Time of occurrence of the event.</param>
public sealed record ProductCreatedEvent(
    Guid EventId,
    Guid ProductId,
    Guid SellerId,
    Guid SubCategoryId,
    ProductStatus Status,
    DateTimeOffset OccurredAt)
    : IDomainEvent;