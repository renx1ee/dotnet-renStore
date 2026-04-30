namespace RenStore.Order.Application.Saga.Contracts.Events;

public sealed record VariantSnapshotFailed(
    Guid   CorrelationId,
    string Reason);