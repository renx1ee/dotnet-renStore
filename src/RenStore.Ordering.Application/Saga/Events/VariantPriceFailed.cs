namespace RenStore.Order.Application.Saga.Events;

public sealed record VariantPriceFailed(
    Guid CorrelationId,
    string Reason);