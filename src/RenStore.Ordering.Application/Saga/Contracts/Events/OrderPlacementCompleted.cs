namespace RenStore.Order.Application.Saga.Contracts.Events;

public sealed record OrderPlacementCompleted(
    Guid CorrelationId,
    Guid OrderId);