using Microsoft.EntityFrameworkCore;
using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Persistence.Write.Repositories;

internal sealed class CountryRepository(DeliveryDbContext context) : ICountryRepository
{
    public async Task CommitAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);

    public async Task AddAsync(Country country, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(country);
        await context.Countries.AddAsync(country, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        int countryId, 
        CancellationToken cancellationToken)
    {
        var address = await GetAsync(countryId, cancellationToken)
                      ?? throw new NotFoundException(typeof(Country), countryId);
        context.Countries.Remove(address);
    }

    public async Task<Country?> GetAsync(
        int countryId, 
        CancellationToken cancellationToken)
    {
        return await context.Countries
            .FirstOrDefaultAsync(x => x.Id == countryId, cancellationToken);
    }
}