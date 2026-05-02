using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.ShipToSortingCenter;

internal sealed class ShipToSortingCenterCommandHandler(
    IDeliveryOrderRepository repository,
    ILogger<ShipToSortingCenterCommandHandler> logger)
    : IRequestHandler<ShipToSortingCenterCommand>
{
    public async Task Handle(
        ShipToSortingCenterCommand request,
        CancellationToken          cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. DeliveryOrderId={Id} SortingCenterId={ScId}",
            nameof(ShipToSortingCenterCommand),
            request.DeliveryOrderId,
            request.SortingCenterId);

        var order = await repository.GetAsync(request.DeliveryOrderId, cancellationToken)
                    ?? throw new NotFoundException(typeof(Domain.Aggregates.DeliveryOrder.DeliveryOrder), request.DeliveryOrderId);

        order.ShipToSortingCenter(request.SortingCenterId, DateTimeOffset.UtcNow);

        await repository.SaveAsync(order, cancellationToken);

        logger.LogInformation(
            "DeliveryOrder shipped to sorting center. DeliveryOrderId={Id}",
            order.Id);
    }
}