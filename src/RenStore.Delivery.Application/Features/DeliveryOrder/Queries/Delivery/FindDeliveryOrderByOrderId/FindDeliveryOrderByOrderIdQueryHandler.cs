using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Queries.Delivery.FindDeliveryOrderByOrderId;

internal sealed class FindDeliveryOrderByOrderIdQueryHandler(
    IDeliveryOrderQuery query,
    ILogger<FindDeliveryOrderByOrderIdQueryHandler> logger)
    : IRequestHandler<FindDeliveryOrderByOrderIdQuery, DeliveryOrderReadModel?>
{
    public async Task<DeliveryOrderReadModel?> Handle(
        FindDeliveryOrderByOrderIdQuery request,
        CancellationToken               cancellationToken)
    {
        logger.LogInformation(
            "Handling {Query}. OrderId={OrderId}",
            nameof(FindDeliveryOrderByOrderIdQuery), request.OrderId);

        return await query.FindByOrderIdAsync(request.OrderId, cancellationToken);
    }
}