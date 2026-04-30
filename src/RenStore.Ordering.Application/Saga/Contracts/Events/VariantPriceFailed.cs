namespace RenStore.Order.Application.Saga.Contracts.Events;

public sealed record VariantPriceFailed(
    Guid   CorrelationId,
    string Reason);