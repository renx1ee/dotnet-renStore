namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.MarkAsAwaitingPickup;

public sealed record MarkAsAwaitingPickupCommand(
    Guid DeliveryOrderId,
    long PickupPointId) 
    : IRequest;