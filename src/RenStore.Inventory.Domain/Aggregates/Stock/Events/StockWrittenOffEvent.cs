using RenStore.Inventory.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Inventory.Domain.Aggregates.Stock.Events;

public sealed record StockWrittenOffEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    WriteOffReason Reason,
    Guid StockId,
    int Count)
    : IDomainEvent;