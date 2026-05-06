using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.Return;

internal sealed class ReturnDeliveryOrderCommandHandler(
    IDeliveryOrderRepository repository,
    ILogger<ReturnDeliveryOrderCommandHandler> logger)
    : IRequestHandler<ReturnDeliveryOrderCommand>
{
    public async Task Handle(
        ReturnDeliveryOrderCommand request,
        CancellationToken          cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. DeliveryOrderId={Id}",
            nameof(ReturnDeliveryOrderCommand),
            request.DeliveryOrderId);

        var order = await repository.GetAsync(request.DeliveryOrderId, cancellationToken)
                    ?? throw new NotFoundException(typeof(Domain.Aggregates.DeliveryOrder.DeliveryOrder), request.DeliveryOrderId);

        order.Return(DateTimeOffset.UtcNow);

        await repository.SaveAsync(order, cancellationToken);

        logger.LogInformation(
            "DeliveryOrder returned. DeliveryOrderId={Id}", order.Id);
    }
}