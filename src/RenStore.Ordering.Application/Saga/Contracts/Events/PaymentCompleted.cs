namespace RenStore.Order.Application.Saga.Contracts.Events;

public sealed record PaymentCompleted(
    Guid CorrelationId,
    Guid OrderId);