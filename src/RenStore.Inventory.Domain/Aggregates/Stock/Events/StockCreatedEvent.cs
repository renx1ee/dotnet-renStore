using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Inventory.Domain.Aggregates.Stock.Events;

public sealed record StockCreatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid StockId,
    Guid VariantId,
    Guid SizeId,
    int InitialStock)
    : IDomainEvent;