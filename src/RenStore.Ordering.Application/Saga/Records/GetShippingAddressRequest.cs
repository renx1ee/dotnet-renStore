namespace RenStore.Order.Application.Saga.Records;

public sealed record GetShippingAddressRequest(
    Guid CorrelationId,
    Guid CustomerId) 
    : CorrelatedBy<Guid>;