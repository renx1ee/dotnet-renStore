namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.Return;

public sealed record ReturnDeliveryOrderCommand(Guid DeliveryOrderId) : IRequest;