using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;

public interface IReviewComplainRepository
{
    Task<Guid> CreateAsync(ReviewComplainEntity complain, CancellationToken cancellationToken);
    Task UpdateAsync(ReviewComplainEntity complain, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<ReviewComplainEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        ReviewComplainSortBy sortBy = ReviewComplainSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);

    Task<ReviewComplainEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<ReviewComplainEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<ReviewComplainEntity?>> FindByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        ReviewComplainSortBy sortBy = ReviewComplainSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);

    Task<IEnumerable<ReviewComplainEntity?>> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        ReviewComplainSortBy sortBy = ReviewComplainSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    
    
}