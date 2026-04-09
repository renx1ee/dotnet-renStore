namespace RenStore.Inventory.Contracts.Events;

public sealed record StockAvailabilityChangedIntegrationEvent(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid SizeId,
    int Count);