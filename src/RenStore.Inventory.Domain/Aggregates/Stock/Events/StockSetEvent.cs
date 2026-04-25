using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Inventory.Domain.Aggregates.Stock.Events;

public sealed record StockSetEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid StockId,
    int NewStock)
    : IDomainEvent;