namespace RenStore.Order.Application.Saga.Contracts.Events;

public sealed record PaymentFailed(
    Guid   CorrelationId,
    string Reason);