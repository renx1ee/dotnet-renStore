using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.MarkAsAwaitingPickup;

internal sealed class MarkAsAwaitingPickupCommandHandler(
    IDeliveryOrderRepository repository,
    ILogger<MarkAsAwaitingPickupCommandHandler> logger)
    : IRequestHandler<MarkAsAwaitingPickupCommand>
{
    public async Task Handle(
        MarkAsAwaitingPickupCommand request,
        CancellationToken           cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. DeliveryOrderId={Id} PickupPointId={PpId}",
            nameof(MarkAsAwaitingPickupCommand),
            request.DeliveryOrderId,
            request.PickupPointId);

        var order = await repository.GetAsync(request.DeliveryOrderId, cancellationToken)
                    ?? throw new NotFoundException(typeof(Domain.Aggregates.DeliveryOrder.DeliveryOrder), request.DeliveryOrderId);

        order.MarkAsAwaitingPickup(request.PickupPointId, DateTimeOffset.UtcNow);

        await repository.SaveAsync(order, cancellationToken);

        logger.LogInformation(
            "DeliveryOrder awaiting pickup. DeliveryOrderId={Id}",
            order.Id);
    }
}