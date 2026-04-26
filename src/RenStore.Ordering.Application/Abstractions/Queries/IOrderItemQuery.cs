using RenStore.Order.Application.Enums;
using RenStore.Order.Domain.ReadModels;

namespace RenStore.Order.Application.Abstractions.Queries;

public interface IOrderItemQuery
{
    Task<OrderItemReadModel?> FindByIdAsync(
        Guid orderItemId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<OrderItemReadModel>> FindByOrderIdAsync(
        Guid orderId,
        OrderItemSortBy sortBy = OrderItemSortBy.CreatedAt,
        uint page = 1,
        uint pageSize = 25,
        bool descending = true,
        bool onlyActive = false,
        bool? isDeleted = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OrderItemReadModel>> FindByVariantIdAsync(
        Guid variantId,
        OrderItemSortBy sortBy = OrderItemSortBy.CreatedAt,
        uint page = 1,
        uint pageSize = 25,
        bool descending = true,
        bool onlyActive = false,
        bool? isDeleted = null,
        CancellationToken cancellationToken = default);
}