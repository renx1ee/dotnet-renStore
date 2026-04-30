namespace RenStore.Order.Application.Saga.Contracts.Events;

public sealed record ShippingAddressFailed(
    Guid   CorrelationId,
    string Reason);