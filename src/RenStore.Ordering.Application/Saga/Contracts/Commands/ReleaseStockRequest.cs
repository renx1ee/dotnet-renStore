namespace RenStore.Order.Application.Saga.Contracts.Commands;

public sealed record ReleaseStockRequest(
    Guid CorrelationId,
    Guid VariantId,
    Guid SizeId,
    int  Quantity);