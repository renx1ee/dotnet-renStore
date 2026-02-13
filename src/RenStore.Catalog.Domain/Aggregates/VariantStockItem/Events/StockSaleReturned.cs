namespace RenStore.Catalog.Domain.Aggregates.VariantStockItem.Events;

public record StockSaleReturned(
    DateTimeOffset OccurredAt,
    Guid StockId,
    int Count);