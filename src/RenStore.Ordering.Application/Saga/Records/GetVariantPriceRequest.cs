namespace RenStore.Order.Application.Saga.Records;

public sealed record GetVariantPriceRequest(
    Guid CorrelationId,
    Guid VariantId,
    Guid SizeId) 
    : CorrelatedBy<Guid>;