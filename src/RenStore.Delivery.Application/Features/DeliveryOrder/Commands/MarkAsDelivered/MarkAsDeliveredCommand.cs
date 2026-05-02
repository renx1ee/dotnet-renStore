namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.MarkAsDelivered;

public sealed record MarkAsDeliveredCommand(Guid DeliveryOrderId) : IRequest;