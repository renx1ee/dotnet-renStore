namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.ShipToSortingCenter;

public sealed record ShipToSortingCenterCommand(
    Guid DeliveryOrderId,
    long SortingCenterId) 
    : IRequest;