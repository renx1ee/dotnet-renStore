namespace RenStore.Delivery.Application.Features.DeliveryOrder.Create;

internal sealed class CreateDeliveryOrderCommandHandler(
    IDeliveryOrderRepository repository,
    ILogger<CreateDeliveryOrderCommandHandler> logger)
    : IRequestHandler<CreateDeliveryOrderCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateDeliveryOrderCommand request,
        CancellationToken          cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. OrderId={OrderId} TariffId={TariffId}",
            nameof(CreateDeliveryOrderCommand),
            request.OrderId,
            request.DeliveryTariffId);

        var order = Domain.Aggregates.DeliveryOrder.DeliveryOrder.Create(
            orderId:          request.OrderId,
            deliveryTariffId: request.DeliveryTariffId,
            now:              DateTimeOffset.UtcNow);

        await repository.SaveAsync(order, cancellationToken);

        logger.LogInformation(
            "DeliveryOrder created. DeliveryOrderId={Id} OrderId={OrderId}",
            order.Id, order.OrderId);

        return order.Id;
    }
}