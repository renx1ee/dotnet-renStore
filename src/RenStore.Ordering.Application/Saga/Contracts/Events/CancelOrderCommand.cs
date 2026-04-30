namespace RenStore.Order.Application.Saga.Contracts.Events;

public sealed record CancelOrderCommand(
    Guid   CorrelationId,
    Guid   OrderId,
    string Reason);