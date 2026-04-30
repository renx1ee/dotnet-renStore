namespace RenStore.Order.Application.Saga.Contracts.Commands;

public sealed record InitiateOrderPlacement(
    Guid CorrelationId,
    Guid CustomerId,
    Guid VariantId,
    Guid SizeId,
    int  Quantity);