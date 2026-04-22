namespace RenStore.Order.Application.Saga.Records;

public sealed record VariantPriceFailed(
    Guid CorrelationId,
    string Reason) 
    : CorrelatedBy<Guid>;