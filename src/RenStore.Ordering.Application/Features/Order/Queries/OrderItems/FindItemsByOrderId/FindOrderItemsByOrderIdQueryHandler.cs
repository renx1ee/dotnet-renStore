using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Order.Application.Abstractions.Queries;
using RenStore.Order.Domain.ReadModels;

namespace RenStore.Order.Application.Features.Order.Queries.OrderItems.FindItemsByOrderId;

internal sealed class FindOrderItemsByOrderIdQueryHandler(
    IOrderItemQuery orderItemQuery,
    ILogger<FindOrderItemsByOrderIdQueryHandler> logger)
    : IRequestHandler<FindOrderItemsByOrderIdQuery, IReadOnlyList<OrderItemReadModel>>
{
    public async Task<IReadOnlyList<OrderItemReadModel>> Handle(
        FindOrderItemsByOrderIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Fetching order items by order id. OrderId: {OrderId}, Page: {Page}, PageSize: {PageSize}, SortBy: {SortBy}, Descending: {Descending}, OnlyActive: {OnlyActive}, IsDeleted: {IsDeleted}",
            request.OrderId,
            request.Page,
            request.PageSize,
            request.SortBy,
            request.Descending,
            request.OnlyActive,
            request.IsDeleted);

        var result = await orderItemQuery.FindByOrderIdAsync(
            request.OrderId,
            request.SortBy,
            request.Page,
            request.PageSize,
            request.Descending,
            request.OnlyActive,
            request.IsDeleted,
            cancellationToken);

        logger.LogInformation(
            "Fetched {Count} order items for order. OrderId: {OrderId}",
            result.Count,
            request.OrderId);

        return result;
    }
}