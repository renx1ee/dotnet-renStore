using RenStore.Catalog.Domain.Entities;

namespace RenStore.Catalog.Domain.Interfaces.Repository;

public interface IColorRepository
{
    Task CommitAsync(CancellationToken cancellationToken);

    Task AddAsync(
        Color product,
        CancellationToken cancellationToken);

    Task<Color?> GetAsync(
        int colorId,
        CancellationToken cancellationToken);

    Task<bool> IsExists(
        int colorId,
        CancellationToken cancellationToken);
}