namespace RenStore.Order.Application.Saga.Records;

public sealed record VariantPriceReceived(
    Guid CorrelationId,
    decimal PriceAmount,
    string Currency,
    string ProductNameSnapshot) 
    : CorrelatedBy<Guid>;