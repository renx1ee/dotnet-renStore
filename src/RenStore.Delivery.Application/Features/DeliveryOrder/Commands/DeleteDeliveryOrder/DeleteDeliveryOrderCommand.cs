namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.DeleteDeliveryOrder;

public sealed record DeleteDeliveryOrderCommand(Guid DeliveryOrderId) : IRequest;