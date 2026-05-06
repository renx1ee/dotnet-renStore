using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.SortAtSortingCenter;

internal sealed class SortAtSortingCenterCommandHandler(
    IDeliveryOrderRepository repository,
    ILogger<SortAtSortingCenterCommandHandler> logger)
    : IRequestHandler<SortAtSortingCenterCommand>
{
    public async Task Handle(
        SortAtSortingCenterCommand request,
        CancellationToken          cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. DeliveryOrderId={Id} SortingCenterId={ScId}",
            nameof(SortAtSortingCenterCommand),
            request.DeliveryOrderId,
            request.SortingCenterId);

        var order = await repository.GetAsync(request.DeliveryOrderId, cancellationToken)
                    ?? throw new NotFoundException(typeof(Domain.Aggregates.DeliveryOrder.DeliveryOrder), request.DeliveryOrderId);

        order.SortAtSortingCenter(request.SortingCenterId, DateTimeOffset.UtcNow);

        await repository.SaveAsync(order, cancellationToken);

        logger.LogInformation(
            "DeliveryOrder sorted at sorting center. DeliveryOrderId={Id}",
            order.Id);
    }
}