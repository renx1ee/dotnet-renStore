namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.ShipToPickupPoint;

public sealed record ShipToPickupPointCommand(
    Guid DeliveryOrderId,
    long PickupPointId) 
    : IRequest;