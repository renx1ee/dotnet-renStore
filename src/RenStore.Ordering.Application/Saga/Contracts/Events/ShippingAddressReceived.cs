namespace RenStore.Order.Application.Saga.Contracts.Events;

public sealed record ShippingAddressReceived(
    Guid   CorrelationId,
    string ShippingAddress);