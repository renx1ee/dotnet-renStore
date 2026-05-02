namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.MarkAsAssembling;

public sealed record MarkAsAssemblingBySellerCommand(
    Guid DeliveryOrderId) 
    : IRequest;