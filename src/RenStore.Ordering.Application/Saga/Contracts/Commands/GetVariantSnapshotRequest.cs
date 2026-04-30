namespace RenStore.Order.Application.Saga.Contracts.Commands;

public sealed record GetVariantSnapshotRequest(
    Guid CorrelationId,
    Guid VariantId,
    Guid SizeId);