using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;

public interface IReviewRepository
{
    Task<Guid> CreateAsync(ReviewEntity review, CancellationToken cancellationToken);
    
    Task UpdateAsync(ReviewEntity review, CancellationToken cancellationToken);
    
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<ReviewEntity?>> FindAllAsync(
        CancellationToken cancellationToken,
        ReviewSortBy sortBy = ReviewSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);

    Task<ReviewEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<ReviewEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<ReviewEntity?>> FindByProductVariantIdAsync(
        Guid productId,
        CancellationToken cancellationToken,
        ReviewSortBy sortBy = ReviewSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);

    Task<IEnumerable<ReviewEntity?>> GetByProductVariantIdAsync(
        Guid productId,
        CancellationToken cancellationToken,
        ReviewSortBy sortBy = ReviewSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);

    Task<IEnumerable<ReviewEntity?>> FindByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        ReviewSortBy sortBy = ReviewSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    
    Task<IEnumerable<ReviewEntity?>> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        ReviewSortBy sortBy = ReviewSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
}