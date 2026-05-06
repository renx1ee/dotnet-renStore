namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.SortAtSortingCenter;

public sealed record SortAtSortingCenterCommand(
    Guid DeliveryOrderId,
    long SortingCenterId) 
    : IRequest;