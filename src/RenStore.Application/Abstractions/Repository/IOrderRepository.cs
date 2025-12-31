using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;

public interface IOrderRepository
{
    Task<Guid> CreateAsync(OrderEntity order, CancellationToken cancellationToken);

    Task UpdateAsync(OrderEntity order, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<OrderEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        OrderSortBy sortBy = OrderSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);

    Task<OrderEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<OrderEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<OrderEntity>> FindByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        OrderSortBy sortBy = OrderSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);

    Task<IEnumerable<OrderEntity>> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        OrderSortBy sortBy = OrderSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);

}