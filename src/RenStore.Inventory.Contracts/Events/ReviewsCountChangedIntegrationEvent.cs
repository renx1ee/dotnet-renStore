namespace RenStore.Inventory.Contracts.Events;

public sealed record ReviewsCountChangedIntegrationEvent(
    DateTimeOffset OccurredAt,
    Guid ProductId,
    Guid VariantId,
    int Count);