using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Order.Application.Abstractions.Queries;
using RenStore.Order.Domain.ReadModels;

namespace RenStore.Order.Application.Features.Order.Queries.Orders.FindByCustomerId;

internal sealed class FindOrdersByCustomerIdQueryHandler(
    IOrderQuery orderQuery,
    ILogger<FindOrdersByCustomerIdQueryHandler> logger)
    : IRequestHandler<FindOrdersByCustomerIdQuery, IReadOnlyList<OrderReadModel>>
{
    public async Task<IReadOnlyList<OrderReadModel>> Handle(
        FindOrdersByCustomerIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Fetching orders by customer id. CustomerId: {CustomerId}, Page: {Page}, PageSize: {PageSize}, SortBy: {SortBy}, Descending: {Descending}, OnlyActive: {OnlyActive}, IsDeleted: {IsDeleted}",
            request.CustomerId,
            request.Page,
            request.PageSize,
            request.SortBy,
            request.Descending,
            request.OnlyActive,
            request.IsDeleted);

        var result = await orderQuery.FindByCustomerIdAsync(
            request.CustomerId,
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
            request.CustomerId);

        return result;
    }
}