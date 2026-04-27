namespace RenStore.Order.Application.Saga.Events;

public sealed record VariantPriceReceived(
    Guid CorrelationId,
    decimal PriceAmount,
    string Currency,
    string ProductNameSnapshot);