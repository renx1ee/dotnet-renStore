namespace RenStore.Order.Application.Saga.Events;

public sealed record OrderCreationFailed(
    Guid CorrelationId,
    string Reason);