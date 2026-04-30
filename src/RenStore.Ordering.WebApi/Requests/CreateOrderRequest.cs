namespace RenStore.Ordering.WebApi.Requests;

public sealed record CreateOrderRequest(
    Guid VariantId,
    Guid SizeId,
    int  Quantity);