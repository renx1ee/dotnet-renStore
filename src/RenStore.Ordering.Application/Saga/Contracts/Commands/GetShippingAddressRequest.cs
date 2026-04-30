namespace RenStore.Order.Application.Saga.Contracts.Commands;

public sealed record GetShippingAddressRequest(
    Guid CorrelationId,
    Guid CustomerId);