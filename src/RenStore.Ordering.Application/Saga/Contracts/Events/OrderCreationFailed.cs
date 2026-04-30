namespace RenStore.Order.Application.Saga.Contracts.Events;

public sealed record OrderCreationFailed(
    Guid   CorrelationId,
    string Reason);