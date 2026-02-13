namespace RenStore.Catalog.Domain.Aggregates.VariantStockItem.Events;

public record StockCreated(
    DateTimeOffset OccurredAt,
    Guid StockId,
    Guid VariantId,
    int InitialStock);