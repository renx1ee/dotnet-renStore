using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Order.Application.Abstractions.Queries;
using RenStore.Order.Application.Features.Order.Queries.Orders.FindById;
using RenStore.Order.Domain.ReadModels;

namespace RenStore.Order.Application.Features.Order.Queries.FindById;

internal sealed class FindOrderByIdQueryHandler(
    IOrderQuery orderQuery,
    ILogger<FindOrderByIdQueryHandler> logger)
    : IRequestHandler<FindOrderByIdQuery, OrderReadModel?>
{
    public async Task<OrderReadModel?> Handle(
        FindOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Fetching order by id. OrderId: {OrderId}",
            request.OrderId);

        var result = await orderQuery.FindByIdAsync(
            request.OrderId,
            cancellationToken);

        if (result is null)
            logger.LogWarning(
                "Order not found. OrderId: {OrderId}",
                request.OrderId);
        else
            logger.LogInformation(
                "Order fetched successfully. OrderId: {OrderId}",
                request.OrderId);

        return result;
    }
}