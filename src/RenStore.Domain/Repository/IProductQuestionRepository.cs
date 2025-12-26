using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;

public interface IProductQuestionRepository
{
    Task<Guid> CreateAsync(ProductQuestionEntity question, CancellationToken cancellationToken);

    Task UpdateAsync(ProductQuestionEntity question, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<ProductQuestionEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductQuestionSortBy sortBy = ProductQuestionSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false,
        bool? isApproved = null);

    Task<ProductQuestionEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<ProductQuestionEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<ProductQuestionEntity?> FindByUserIdAsync(string userId, CancellationToken cancellationToken);

    Task<ProductQuestionEntity> GetByUserIdAsync(string userId, CancellationToken cancellationToken);
}