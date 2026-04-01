using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Inventory.Domain.Aggregates.Stock.Events;

public sealed record StockSetEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid StockId,
    Guid SizeId,
    Guid VariantSizeId,
    int NewStock)
    : IDomainEvent;