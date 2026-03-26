using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Inventory.Domain.Aggregates.Stock.Events;

public sealed record StockSaleReturnedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid StockId,
    int Count)
    : IDomainEvent;