namespace RenStore.Order.Application.Saga.Events;

public sealed record OrderCreated(
    Guid CorrelationId,
    Guid OrderId);