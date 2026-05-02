using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Abstractions.Projections;

public interface ICityProjection
{
    Task CommitAsync(CancellationToken cancellationToken);
    Task AddAsync(CityReadModel city, CancellationToken cancellationToken);
    Task UpdateAsync(DateTimeOffset now, int cityId, string name, string nameRu, CancellationToken cancellationToken);
    Task SetDeletedAsync(DateTimeOffset now, int cityId, CancellationToken cancellationToken);
}