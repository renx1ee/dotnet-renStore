namespace RenStore.Order.Application.Saga.Events;

public sealed record SagaTimeout(
    Guid CorrelationId);