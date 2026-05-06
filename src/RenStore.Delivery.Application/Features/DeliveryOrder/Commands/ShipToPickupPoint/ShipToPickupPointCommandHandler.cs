using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.ShipToPickupPoint;

internal sealed class ShipToPickupPointCommandHandler(
    IDeliveryOrderRepository repository,
    ILogger<ShipToPickupPointCommandHandler> logger)
    : IRequestHandler<ShipToPickupPointCommand>
{
    public async Task Handle(
        ShipToPickupPointCommand request,
        CancellationToken        cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. DeliveryOrderId={Id} PickupPointId={PpId}",
            nameof(ShipToPickupPointCommand),
            request.DeliveryOrderId,
            request.PickupPointId);

        var order = await repository.GetAsync(request.DeliveryOrderId, cancellationToken)
                    ?? throw new NotFoundException(typeof(Domain.Aggregates.DeliveryOrder.DeliveryOrder), request.DeliveryOrderId);

        order.ShipToPickupPoint(request.PickupPointId, DateTimeOffset.UtcNow);

        await repository.SaveAsync(order, cancellationToken);

        logger.LogInformation(
            "DeliveryOrder shipped to pickup point. DeliveryOrderId={Id} PickupPointId={PpId}",
            order.Id, request.PickupPointId);
    }
}