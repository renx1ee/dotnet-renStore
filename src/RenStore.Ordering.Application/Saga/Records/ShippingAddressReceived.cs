namespace RenStore.Order.Application.Saga.Records;

public sealed record ShippingAddressReceived(
    Guid CorrelationId,
    string ShippingAddress) 
    : CorrelatedBy<Guid>;