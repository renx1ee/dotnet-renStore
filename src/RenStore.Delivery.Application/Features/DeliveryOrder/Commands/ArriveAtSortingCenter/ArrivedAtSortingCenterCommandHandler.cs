using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.ArriveAtSortingCenter;

internal sealed class ArrivedAtSortingCenterCommandHandler(
    IDeliveryOrderRepository repository,
    ILogger<ArrivedAtSortingCenterCommandHandler> logger)
    : IRequestHandler<ArrivedAtSortingCenterCommand>
{
    public async Task Handle(
        ArrivedAtSortingCenterCommand request,
        CancellationToken             cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. DeliveryOrderId={Id} SortingCenterId={ScId}",
            nameof(ArrivedAtSortingCenterCommand),
            request.DeliveryOrderId,
            request.SortingCenterId);

        var order = await repository.GetAsync(request.DeliveryOrderId, cancellationToken)
                    ?? throw new NotFoundException(typeof(Domain.Aggregates.DeliveryOrder.DeliveryOrder), request.DeliveryOrderId);

        order.MarkAsArrivedAtSortingCenter(request.SortingCenterId, DateTimeOffset.UtcNow);

        await repository.SaveAsync(order, cancellationToken);

        logger.LogInformation(
            "DeliveryOrder arrived at sorting center. DeliveryOrderId={Id} SortingCenterId={ScId}",
            order.Id, request.SortingCenterId);
    }
}