namespace RenStore.Order.Application.Saga.Commands;

public sealed record CreateOrderCommand(
    Guid CorrelationId,
    Guid CustomerId,
    Guid VariantId,
    Guid SizeId,
    int Quantity,
    decimal PriceAmount,
    string Currency,
    string ProductNameSnapshot,
    string ShippingAddress);