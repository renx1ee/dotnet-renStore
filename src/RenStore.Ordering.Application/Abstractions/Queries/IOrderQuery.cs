using RenStore.Order.Application.Enums;
using RenStore.Order.Domain.ReadModels;

namespace RenStore.Order.Application.Abstractions.Queries;

public interface IOrderQuery
{
    Task<OrderReadModel?> FindByIdAsync(
        Guid orderId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<OrderReadModel>> FindByCustomerIdAsync(
        Guid customerId,
        OrderSortBy sortBy = OrderSortBy.CreatedAt,
        uint page = 1,
        uint pageSize = 25,
        bool descending = true,
        bool onlyActive = false,
        bool? isDeleted = null,
        CancellationToken cancellationToken = default);
}