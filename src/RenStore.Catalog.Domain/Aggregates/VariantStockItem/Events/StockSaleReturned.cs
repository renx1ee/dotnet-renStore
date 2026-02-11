namespace RenStore.Catalog.Domain.Aggregates.StockItem.Events;

public record StockSaleReturned(
    DateTimeOffset OccurredAt,
    Guid StockId,
    int Count);