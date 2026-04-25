using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Inventory.Domain.Aggregates.Stock.Events;

public sealed record StockAddedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid StockId,
    int Count)
    : IDomainEvent;

public sealed record StockDecreasedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid StockId,
    int Count)
    : IDomainEvent;

public sealed record StockReservationReturnedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid StockId,
    int Count)
    : IDomainEvent;