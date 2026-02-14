namespace RenStore.Inventory.Domain.Aggregates.Stock.Events;

public record StockSet(
    DateTimeOffset OccurredAt,
    Guid SizeId,
    Guid VariantSizeId,
    int NewStock);