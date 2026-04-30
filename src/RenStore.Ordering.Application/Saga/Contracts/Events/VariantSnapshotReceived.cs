namespace RenStore.Order.Application.Saga.Contracts.Events;

public sealed record VariantSnapshotReceived(
    Guid    CorrelationId,
    decimal PriceAmount,
    string  Currency,
    string  ProductNameSnapshot);