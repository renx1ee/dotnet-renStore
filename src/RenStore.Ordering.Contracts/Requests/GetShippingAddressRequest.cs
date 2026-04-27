namespace RenStore.Ordering.Contracts.Requests;

public sealed record GetShippingAddressRequest(
    Guid CorrelationId,
    Guid CustomerId);