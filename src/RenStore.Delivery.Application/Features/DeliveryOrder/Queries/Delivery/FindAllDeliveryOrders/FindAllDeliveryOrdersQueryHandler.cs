using RenStore.Delivery.Application.Abstractions.Queries;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Queries.Delivery.FindAllDeliveryOrders;

internal sealed class FindAllDeliveryOrdersQueryHandler(
    IDeliveryOrderQuery query,
    ILogger<FindAllDeliveryOrdersQueryHandler> logger)
    : IRequestHandler<FindAllDeliveryOrdersQuery, IReadOnlyList<DeliveryOrderReadModel>>
{
    public async Task<IReadOnlyList<DeliveryOrderReadModel>> Handle(
        FindAllDeliveryOrdersQuery request,
        CancellationToken          cancellationToken)
    {
        logger.LogInformation(
            "Handling {Query}. Page={Page} PageSize={PageSize} Status={Status}",
            nameof(FindAllDeliveryOrdersQuery),
            request.Page, request.PageSize, request.Status);

        var result = await query.FindAllAsync(
            request.SortBy, 
            request.Page, 
            request.PageSize,
            request.Descending, 
            request.Status, 
            cancellationToken);

        logger.LogInformation("Fetched {Count} delivery orders.", result.Count);

        return result;
    }
}