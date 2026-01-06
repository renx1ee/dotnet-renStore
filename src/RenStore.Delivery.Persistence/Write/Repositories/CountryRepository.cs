using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Persistence.Write.Repositories;

internal sealed class CountryRepository(DeliveryDbContext context) 
    : RenStore.Delivery.Domain.Interfaces.ICountryRepository
{
    private readonly DeliveryDbContext _context = context 
                                                     ?? throw new ArgumentNullException(nameof(context));
    
    public async Task<int> AddAsync(
        Country country, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(country);

        var result = await this._context.Countries.AddAsync(country, cancellationToken);
        
        return result.Entity.Id;
    }

    public async Task AddRangeAsync(
        IReadOnlyCollection<Country> countries, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(countries);

        var countriesList = countries as IList<Country> ?? countries.ToList();

        if (countriesList.Count == 0)
            return;
        
        await this._context.Countries.AddRangeAsync(countries, cancellationToken);
    }

    public void Remove(Country country)
    {
        ArgumentNullException.ThrowIfNull(country);

        this._context.Countries.Remove(country);
    }

    public void RemoveRange(IReadOnlyCollection<Country> countries)
    {
        ArgumentNullException.ThrowIfNull(countries);
        
        var countriesList = countries as IList<Country> ?? countries.ToList();

        if (countriesList.Count == 0)
            return;
        
        this._context.Countries.RemoveRange(countries);
    }
}