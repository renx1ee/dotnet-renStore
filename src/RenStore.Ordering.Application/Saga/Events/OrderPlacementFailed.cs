namespace RenStore.Order.Application.Saga.Events;

public sealed record OrderPlacementFailed(
    Guid CorrelationId,
    string Reason);