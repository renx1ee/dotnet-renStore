using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;

public interface IShoppingCartRepository
{
    Task<Guid> CreateAsync(ShoppingCartEntity cart, CancellationToken cancellationToken);

    Task UpdateAsync(ShoppingCartEntity cart, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<ShoppingCartEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        ShoppingCartSortBy sortBy = ShoppingCartSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);

    Task<ShoppingCartEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<ShoppingCartEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<ShoppingCartEntity?> FindByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        ShoppingCartSortBy sortBy = ShoppingCartSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);

    Task<ShoppingCartEntity?> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        ShoppingCartSortBy sortBy = ShoppingCartSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
}