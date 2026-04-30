namespace RenStore.Order.Application.Saga.Contracts.Events;

public sealed record SagaTimeout(
    Guid CorrelationId);