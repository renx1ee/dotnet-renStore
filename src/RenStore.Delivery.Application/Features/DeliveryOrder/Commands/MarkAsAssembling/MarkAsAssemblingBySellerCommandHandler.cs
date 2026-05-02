using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.MarkAsAssembling;

internal sealed class MarkAsAssemblingBySellerCommandHandler(
    IDeliveryOrderRepository repository,
    ILogger<MarkAsAssemblingBySellerCommandHandler> logger)
    : IRequestHandler<MarkAsAssemblingBySellerCommand>
{
    public async Task Handle(
        MarkAsAssemblingBySellerCommand request,
        CancellationToken               cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. DeliveryOrderId={Id}",
            nameof(MarkAsAssemblingBySellerCommand), request.DeliveryOrderId);

        var order = await repository.GetAsync(request.DeliveryOrderId, cancellationToken)
                    ?? throw new NotFoundException(typeof(Domain.Aggregates.DeliveryOrder.DeliveryOrder), request.DeliveryOrderId);

        order.MarkAsAssemblingBySeller(DateTimeOffset.UtcNow);

        await repository.SaveAsync(order, cancellationToken);

        logger.LogInformation(
            "DeliveryOrder marked as assembling. DeliveryOrderId={Id}", order.Id);
    }
}