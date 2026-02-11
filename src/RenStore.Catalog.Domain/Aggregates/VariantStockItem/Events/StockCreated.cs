namespace RenStore.Catalog.Domain.Aggregates.StockItem.Events;

public record StockCreated(
    DateTimeOffset OccurredAt,
    Guid StockId,
    Guid VariantId,
    int InitialStock);