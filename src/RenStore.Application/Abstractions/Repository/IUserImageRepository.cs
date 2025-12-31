using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;

public interface IUserImageRepository
{
    Task<Guid> CreateAsync(UserImageEntity image, CancellationToken cancellationToken);

    Task UpdateAsync(UserImageEntity image, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<UserImageEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false,
        UserImageSortBy sortBy = UserImageSortBy.Id);

    Task<UserImageEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<UserImageEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}