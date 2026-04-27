namespace RenStore.Order.Application.Saga.Events;

public sealed record OrderPlacementCompleted(
    Guid CorrelationId,
    Guid OrderId);