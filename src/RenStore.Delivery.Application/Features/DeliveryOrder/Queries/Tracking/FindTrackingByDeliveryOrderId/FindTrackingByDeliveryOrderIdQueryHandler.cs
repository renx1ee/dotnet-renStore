using RenStore.Delivery.Application.Abstractions.Queries;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Queries.Tracking.FindTrackingByDeliveryOrderId;

internal sealed class FindTrackingByDeliveryOrderIdQueryHandler(
    IDeliveryTrackingQuery query,
    ILogger<FindTrackingByDeliveryOrderIdQueryHandler> logger)
    : IRequestHandler<FindTrackingByDeliveryOrderIdQuery, IReadOnlyList<DeliveryTrackingReadModel>>
{
    public async Task<IReadOnlyList<DeliveryTrackingReadModel>> Handle(
        FindTrackingByDeliveryOrderIdQuery request,
        CancellationToken                  cancellationToken)
    {
        logger.LogInformation(
            "Handling {Query}. DeliveryOrderId={Id}",
            nameof(FindTrackingByDeliveryOrderIdQuery), request.DeliveryOrderId);

        var result = await query.FindByDeliveryOrderIdAsync(
            request.DeliveryOrderId, cancellationToken);

        logger.LogInformation(
            "Fetched {Count} tracking records. DeliveryOrderId={Id}",
            result.Count, request.DeliveryOrderId);

        return result;
    }
}