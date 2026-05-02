using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.MarkAsDelivered;

internal sealed class MarkAsDeliveredCommandHandler(
    IDeliveryOrderRepository repository,
    ILogger<MarkAsDeliveredCommandHandler> logger)
    : IRequestHandler<MarkAsDeliveredCommand>
{
    public async Task Handle(
        MarkAsDeliveredCommand request,
        CancellationToken      cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. DeliveryOrderId={Id}",
            nameof(MarkAsDeliveredCommand), request.DeliveryOrderId);

        var order = await repository.GetAsync(request.DeliveryOrderId, cancellationToken)
                    ?? throw new NotFoundException(typeof(Domain.Aggregates.DeliveryOrder.DeliveryOrder), request.DeliveryOrderId);

        order.MarkAsDelivered(DateTimeOffset.UtcNow);

        await repository.SaveAsync(order, cancellationToken);

        logger.LogInformation(
            "DeliveryOrder marked as delivered. DeliveryOrderId={Id}", order.Id);
    }
}