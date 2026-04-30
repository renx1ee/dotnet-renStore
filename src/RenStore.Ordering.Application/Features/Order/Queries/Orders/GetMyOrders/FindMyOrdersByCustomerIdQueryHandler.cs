using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Order.Application.Abstractions.Queries;
using RenStore.Order.Application.Services;
using RenStore.Order.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Order.Application.Features.Order.Queries.Orders.GetMyOrders;

internal sealed class FindMyOrdersByCustomerIdQueryHandler(
    IOrderQuery orderQuery,
    ILogger<FindMyOrdersByCustomerIdQueryHandler> logger,
    ICurrentUserService userService)
    : IRequestHandler<FindMyOrdersByCustomerIdQuery, IReadOnlyList<OrderReadModel>>
{
    public async Task<IReadOnlyList<OrderReadModel>> Handle(
        FindMyOrdersByCustomerIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Fetching orders by customer id. Page: {Page}, PageSize: {PageSize}, SortBy: {SortBy}, Descending: {Descending}, OnlyActive: {OnlyActive}, IsDeleted: {IsDeleted}",
            request.Page,
            request.PageSize,
            request.SortBy,
            request.Descending,
            request.OnlyActive,
            request.IsDeleted);

        var userId = userService.UserId;

        if (userId == Guid.Empty)
            throw new ForbiddenException();
        
        var result = await orderQuery.FindByCustomerIdAsync(
            (Guid)userId!,
            request.SortBy,
            request.Page,
            request.PageSize,
            request.Descending,
            request.OnlyActive,
            request.IsDeleted,
            cancellationToken);

        logger.LogInformation(
            "Fetched {Count} orders for customer. CustomerId: {CustomerId}",
            result.Count,
            userId);

        return result;
    }
}