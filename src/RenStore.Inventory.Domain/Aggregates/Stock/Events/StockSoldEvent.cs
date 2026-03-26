using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Inventory.Domain.Aggregates.Stock.Events;

public sealed record StockSoldEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid StockId,
    int Count)
    : IDomainEvent;