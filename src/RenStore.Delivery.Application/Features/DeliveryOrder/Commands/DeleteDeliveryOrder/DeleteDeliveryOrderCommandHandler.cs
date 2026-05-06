using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.DeleteDeliveryOrder;

internal sealed class DeleteDeliveryOrderCommandHandler(
    IDeliveryOrderRepository repository,
    ILogger<DeleteDeliveryOrderCommandHandler> logger)
    : IRequestHandler<DeleteDeliveryOrderCommand>
{
    public async Task Handle(
        DeleteDeliveryOrderCommand request,
        CancellationToken          cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. DeliveryOrderId={Id}",
            nameof(DeleteDeliveryOrderCommand),
            request.DeliveryOrderId);

        var order = await repository.GetAsync(request.DeliveryOrderId, cancellationToken)
                    ?? throw new NotFoundException(typeof(Domain.Aggregates.DeliveryOrder.DeliveryOrder), request.DeliveryOrderId);

        order.Delete(DateTimeOffset.UtcNow);

        await repository.SaveAsync(order, cancellationToken);

        logger.LogInformation(
            "DeliveryOrder deleted. DeliveryOrderId={Id}", order.Id);
    }
}