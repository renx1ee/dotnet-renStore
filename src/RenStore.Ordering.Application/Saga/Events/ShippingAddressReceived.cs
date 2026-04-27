namespace RenStore.Order.Application.Saga.Events;

public sealed record ShippingAddressReceived(
    Guid CorrelationId,
    string ShippingAddress);