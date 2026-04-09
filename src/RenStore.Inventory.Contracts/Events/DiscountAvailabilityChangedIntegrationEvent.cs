namespace RenStore.Inventory.Contracts.Events;

public sealed record DiscountAvailabilityChangedIntegrationEvent(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    int Count);