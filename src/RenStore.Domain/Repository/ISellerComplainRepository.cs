using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;

public interface ISellerComplainRepository
{
    Task<Guid> CreateAsync(SellerComplainEntity complain, CancellationToken cancellationToken);
    Task UpdateAsync(SellerComplainEntity complain, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<SellerComplainEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        SellerComplainSortBy sortBy = SellerComplainSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    Task<SellerComplainEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<SellerComplainEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<SellerComplainEntity?>> FindByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        SellerComplainSortBy sortBy = SellerComplainSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    Task<IEnumerable<SellerComplainEntity?>> GetByUserIdAsync(
        string userId, 
        CancellationToken cancellationToken,
        SellerComplainSortBy sortBy = SellerComplainSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);

}