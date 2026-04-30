namespace RenStore.Order.Application.Saga.Contracts.Commands;

public sealed record ReserveStockRequest(
    Guid CorrelationId,
    Guid VariantId,
    Guid SizeId,
    int  Quantity);