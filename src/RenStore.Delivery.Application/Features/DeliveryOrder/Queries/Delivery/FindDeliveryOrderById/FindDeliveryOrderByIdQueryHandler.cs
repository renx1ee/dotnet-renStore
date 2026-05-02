using RenStore.Delivery.Application.Abstractions.Queries;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Queries.Delivery.FindDeliveryOrderById;

internal sealed class FindDeliveryOrderByIdQueryHandler(
    IDeliveryOrderQuery query,
    ILogger<FindDeliveryOrderByIdQueryHandler> logger)
    : IRequestHandler<FindDeliveryOrderByIdQuery, DeliveryOrderReadModel?>
{
    public async Task<DeliveryOrderReadModel?> Handle(
        FindDeliveryOrderByIdQuery request,
        CancellationToken          cancellationToken)
    {
        logger.LogInformation(
            "Handling {Query}. DeliveryOrderId={Id}",
            nameof(FindDeliveryOrderByIdQuery), request.DeliveryOrderId);

        var result = await query.FindByIdAsync(request.DeliveryOrderId, cancellationToken);

        if (result is null)
            logger.LogWarning(
                "DeliveryOrder not found. DeliveryOrderId={Id}",
                request.DeliveryOrderId);

        return result;
    }
}