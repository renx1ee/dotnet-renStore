namespace RenStore.Order.Application.Saga.Contracts.Events;

public sealed record OrderPlacementFailed(
    Guid   CorrelationId,
    string Reason);