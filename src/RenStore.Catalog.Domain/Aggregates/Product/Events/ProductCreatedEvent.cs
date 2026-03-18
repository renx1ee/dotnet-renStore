using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Product.Events;

public sealed record ProductCreatedEvent(
    Guid EventId,
    Guid ProductId,
    Guid SellerId,
    Guid SubCategoryId,
    ProductStatus Status,
    DateTimeOffset OccurredAt)
    : IDomainEvent;