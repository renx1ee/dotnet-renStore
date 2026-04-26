using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Order.Application.Abstractions.Queries;
using RenStore.Order.Application.Features.Order.Queries.OrderItems.FindItemsByOrderId;
using RenStore.Order.Application.Services;
using RenStore.Order.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Order.Application.Features.Order.Queries.OrderItems.FindMyItemsByOrderId;

internal sealed class FindMyOrderItemsByOrderIdQueryHandler(
    IOrderQuery orderQuery,
    IOrderItemQuery orderItemQuery,
    ILogger<FindOrderItemsByOrderIdQueryHandler> logger,
    ICurrentUserService userService)
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

        var order = await orderQuery.FindByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
        {
            throw new NotFoundException(
                name: typeof(OrderReadModel), request.OrderId);
        }

        if (order.CustomerId != userService.UserId)
        {
            throw new ForbiddenException("Access to order items is denied.");
        }

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