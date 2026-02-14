namespace RenStore.Inventory.Domain.Aggregates.Stock.Events;

public record StockCreated(
    DateTimeOffset OccurredAt,
    Guid StockId,
    Guid SizeId,
    int InitialStock);