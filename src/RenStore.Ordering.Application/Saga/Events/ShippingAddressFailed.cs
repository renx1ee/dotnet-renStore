namespace RenStore.Order.Application.Saga.Events;

public sealed record ShippingAddressFailed(
    Guid CorrelationId,
    string Reason);