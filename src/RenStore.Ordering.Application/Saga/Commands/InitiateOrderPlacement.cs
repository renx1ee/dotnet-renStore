namespace RenStore.Order.Application.Saga.Commands;

public sealed record InitiateOrderPlacement(
    Guid CorrelationId,
    Guid CustomerId,
    Guid VariantId,
    Guid SizeId,
    int Quantity);