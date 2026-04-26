using MediatR;
using RenStore.Order.Application.Enums;
using RenStore.Order.Domain.ReadModels;

namespace RenStore.Order.Application.Features.Order.Queries.OrderItems.FindMyItemsByOrderId;

public sealed record FindMyOrderItemsByOrderIdQuery(
    Guid OrderId,
    OrderItemSortBy SortBy = OrderItemSortBy.CreatedAt,
    uint Page = 1,
    uint PageSize = 25,
    bool Descending = true,
    bool OnlyActive = false,
    bool? IsDeleted = null) 
    : IRequest<IReadOnlyList<OrderItemReadModel>>;