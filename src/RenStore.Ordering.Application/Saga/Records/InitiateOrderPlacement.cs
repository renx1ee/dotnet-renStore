namespace RenStore.Order.Application.Saga.Records;

public sealed record class InitiateOrderPlacement(
    Guid CorrelationId, // OrderId
    Guid CustomerId,
    Guid VariantId,
    Guid SizeId,
    int Quantity)
    : CorrelatedBy<Guid>;