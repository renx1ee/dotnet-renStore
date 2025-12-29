using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;

public interface IOrderItemRepository
{
    Task<Guid> CreateAsync(OrderItemEntity orderItem, CancellationToken cancellationToken);
    
    Task UpdateAsync(OrderItemEntity orderItem, CancellationToken cancellationToken);
    
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    
    Task<OrderItemEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<OrderItemEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<OrderItemEntity>> FindByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken,
        OrderItemSortBy sortBy = OrderItemSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);

    Task<IEnumerable<OrderItemEntity>> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken,
        OrderItemSortBy sortBy = OrderItemSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);

    Task<IEnumerable<OrderItemEntity>> FindByProductOrderIdAsync(
        Guid productId,
        CancellationToken cancellationToken,
        OrderItemSortBy sortBy = OrderItemSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    
    Task<IEnumerable<OrderItemEntity>> GetByProductIdAsync(
        Guid productId,
        CancellationToken cancellationToken,
        OrderItemSortBy sortBy = OrderItemSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
}