using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Inventory.Contracts.Events;

public sealed record ReviewsCountChangedIntegrationEvent(
    DateTimeOffset OccurredAt,
    Guid ProductId,
    Guid VariantId,
    int AverageRating,
    int Count)
    : IIntegrationEvent;