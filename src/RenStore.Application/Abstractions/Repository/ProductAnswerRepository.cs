using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;

public interface IProductAnswerRepository
{
    Task<Guid> CreateAsync(ProductAnswerEntity productAnswerEntity, CancellationToken cancellationToken);

    Task UpdateAsync(ProductAnswerEntity answer, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<ProductAnswerEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductAnswerSortBy sortBy = ProductAnswerSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false,
        bool? isApproved = null);

    Task<ProductAnswerEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<ProductAnswerEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<ProductAnswerEntity?> FindBySellerIdAsync(string sellerId, CancellationToken cancellationToken);

    Task<ProductAnswerEntity> GetBySellerIdAsync(string userId, CancellationToken cancellationToken);
}