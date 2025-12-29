using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;

public interface IProductVariantComplainRepository
{
    Task<Guid> CreateAsync(ProductVariantComplainEntity complain, CancellationToken cancellationToken);
    Task UpdateAsync(ProductVariantComplainEntity complain, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<ProductVariantComplainEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductVariantComplainSortBy sortBy = ProductVariantComplainSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    Task<ProductVariantComplainEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<ProductVariantComplainEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<ProductVariantComplainEntity?>> FindByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        ProductVariantComplainSortBy sortBy = ProductVariantComplainSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    Task<IEnumerable<ProductVariantComplainEntity?>> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        ProductVariantComplainSortBy sortBy = ProductVariantComplainSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
}