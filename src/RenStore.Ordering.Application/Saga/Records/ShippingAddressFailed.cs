namespace RenStore.Order.Application.Saga.Records;

public sealed record ShippingAddressFailed(
    Guid CorrelationId,
    string Reason) 
    : CorrelatedBy<Guid>;