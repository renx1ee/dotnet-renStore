using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Domain.Interfaces;

public interface ICountryRepository
{
    Task CommitAsync(CancellationToken cancellationToken);
    
    Task AddAsync(Country country, CancellationToken cancellationToken);

    Task DeleteAsync(int countryId, CancellationToken cancellationToken);

    Task<Country?> GetAsync(int countryId, CancellationToken cancellationToken);
}