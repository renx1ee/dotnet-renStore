using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Abstractions.Projections;

public interface ICountryProjection
{
    Task CommitAsync(CancellationToken cancellationToken);
    
    Task AddAsync(CountryReadModel country, CancellationToken cancellationToken);
    
    Task UpdateAsync(DateTimeOffset now, int countryId, string name, string nameRu, string code, string phoneCode, CancellationToken cancellationToken);
    
    Task SetDeletedAsync(DateTimeOffset now, int countryId, CancellationToken cancellationToken);
}