using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Inventory.Domain.Aggregates.Stock.Events;

public record StockSaleReturned(
    DateTimeOffset OccurredAt,
    Guid StockId,
    int Count)
    : IDomainEvent;