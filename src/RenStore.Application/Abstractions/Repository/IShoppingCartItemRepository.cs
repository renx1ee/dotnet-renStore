using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;

public interface IShoppingCartItemRepository
{
    Task<Guid> CreateAsync(ShoppingCartItemEntity item, CancellationToken cancellationToken);
    
    Task UpdateAsync(ShoppingCartItemEntity item, CancellationToken cancellationToken);
    
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<ShoppingCartItemEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        ShoppingCartItemSortBy sortBy = ShoppingCartItemSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);

    Task<ShoppingCartItemEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<ShoppingCartItemEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<ShoppingCartItemEntity>> FindByCartIdAsync(
        Guid cartId,
        CancellationToken cancellationToken,
        ShoppingCartItemSortBy sortBy = ShoppingCartItemSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);

    Task<IEnumerable<ShoppingCartItemEntity>> GetByCartIdAsync(
        Guid cartId,
        CancellationToken cancellationToken,
        ShoppingCartItemSortBy sortBy = ShoppingCartItemSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
}