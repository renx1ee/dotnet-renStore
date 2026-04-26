using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Order.Application.Abstractions.Queries;
using RenStore.Order.Domain.ReadModels;

namespace RenStore.Order.Application.Features.Order.Queries.OrderItems.FindOrderItemById;

internal sealed class FindOrderItemByIdQueryHandler(
    IOrderItemQuery orderItemQuery,
    ILogger<FindOrderItemByIdQueryHandler> logger)
    : IRequestHandler<FindOrderItemByIdQuery, OrderItemReadModel?>
{
    public async Task<OrderItemReadModel?> Handle(
        FindOrderItemByIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Fetching order item by id. OrderItemId: {OrderItemId}",
            request.OrderItemId);

        var result = await orderItemQuery.FindByIdAsync(
            request.OrderItemId,
            cancellationToken);

        if (result is null)
            logger.LogWarning(
                "Order item not found. OrderItemId: {OrderItemId}",
                request.OrderItemId);
        else
            logger.LogInformation(
                "Order item fetched successfully. OrderItemId: {OrderItemId}",
                request.OrderItemId);

        return result;
    }
}