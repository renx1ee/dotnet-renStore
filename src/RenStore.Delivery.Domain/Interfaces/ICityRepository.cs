using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Domain.Interfaces;

public interface ICityRepository
{
    Task CommitAsync(CancellationToken cancellationToken);

    Task AddAsync(City city, CancellationToken cancellationToken);

    Task DeleteAsync(int cityId, CancellationToken cancellationToken);

    Task<City?> GetAsync(int cityId, CancellationToken cancellationToken);
}