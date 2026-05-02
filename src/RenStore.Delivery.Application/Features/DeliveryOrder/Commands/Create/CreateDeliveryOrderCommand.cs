namespace RenStore.Delivery.Application.Features.DeliveryOrder.Create;

public sealed record CreateDeliveryOrderCommand(
    Guid OrderId,
    int  DeliveryTariffId) 
    : IRequest<Guid>;