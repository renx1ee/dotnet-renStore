using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Inventory.Domain.Aggregates.Stock.Events;

public record StockCreated(
    DateTimeOffset OccurredAt,
    Guid StockId,
    Guid SizeId,
    int InitialStock)
    : IDomainEvent;