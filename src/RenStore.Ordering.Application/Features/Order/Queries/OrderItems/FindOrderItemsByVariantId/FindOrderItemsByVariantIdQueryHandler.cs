using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Order.Application.Abstractions.Queries;
using RenStore.Order.Domain.ReadModels;

namespace RenStore.Order.Application.Features.Order.Queries.OrderItems.FindOrderItemsByVariantId;

internal sealed class FindOrderItemsByVariantIdQueryHandler(
    IOrderItemQuery orderItemQuery,
    ILogger<FindOrderItemsByVariantIdQueryHandler> logger)
    : IRequestHandler<FindOrderItemsByVariantIdQuery, IReadOnlyList<OrderItemReadModel>>
{
    public async Task<IReadOnlyList<OrderItemReadModel>> Handle(
        FindOrderItemsByVariantIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Fetching order items by variant id. VariantId: {VariantId}, Page: {Page}, PageSize: {PageSize}, SortBy: {SortBy}, Descending: {Descending}, OnlyActive: {OnlyActive}, IsDeleted: {IsDeleted}",
            request.VariantId,
            request.Page,
            request.PageSize,
            request.SortBy,
            request.Descending,
            request.OnlyActive,
            request.IsDeleted);

        var result = await orderItemQuery.FindByVariantIdAsync(
            request.VariantId,
            request.SortBy,
            request.Page,
            request.PageSize,
            request.Descending,
            request.OnlyActive,
            request.IsDeleted,
            cancellationToken);

        logger.LogInformation(
            "Fetched {Count} order items for variant. VariantId: {VariantId}",
            result.Count,
            request.VariantId);

        return result;
    }
}