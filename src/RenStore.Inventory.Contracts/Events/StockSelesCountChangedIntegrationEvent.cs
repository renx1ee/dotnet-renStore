namespace RenStore.Inventory.Contracts.Events;

public sealed record StockSelesCountChangedIntegrationEvent(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid SizeId,
    int Count);