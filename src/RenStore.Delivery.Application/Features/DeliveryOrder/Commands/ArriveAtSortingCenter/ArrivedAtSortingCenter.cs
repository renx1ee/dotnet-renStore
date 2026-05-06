namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.ArriveAtSortingCenter;

public sealed record ArrivedAtSortingCenterCommand(
    Guid DeliveryOrderId,
    long SortingCenterId) 
    : IRequest;