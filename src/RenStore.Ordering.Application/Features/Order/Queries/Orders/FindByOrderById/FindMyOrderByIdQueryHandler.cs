using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Order.Application.Abstractions.Queries;
using RenStore.Order.Application.Abstractions.Services;
using RenStore.Order.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Order.Application.Features.Order.Queries.Orders.FindByOrderById;

internal sealed class FindMyOrderByIdQueryHandler(
    IOrderQuery orderQuery,
    ILogger<FindMyOrderByIdQueryHandler> logger,
    ICurrentUserService userService)
    : IRequestHandler<FindMyOrderByIdQuery, OrderReadModel?>
{
    public async Task<OrderReadModel?> Handle(
        FindMyOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Fetching order by id. OrderId: {OrderId}",
            request.OrderId);

        var result = await orderQuery.FindByIdAsync(
            request.OrderId,
            cancellationToken);

        if (result is not null && 
            result.CustomerId != userService.UserId)
        {
            throw new ForbiddenException();
        }

        if (result is null)
        {
            logger.LogWarning(
                "Order not found. OrderId: {OrderId}",
                request.OrderId);
        }
        else
        {
            logger.LogInformation(
                "Order fetched successfully. OrderId: {OrderId}",
                request.OrderId);
        }

        return result;
    }
}