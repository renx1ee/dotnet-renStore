namespace RenStore.Inventory.Domain.Aggregates.Stock.Events;

public record StockSaleReturned(
    DateTimeOffset OccurredAt,
    Guid StockId,
    int Count);