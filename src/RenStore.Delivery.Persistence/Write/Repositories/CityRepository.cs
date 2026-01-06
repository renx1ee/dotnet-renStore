using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Persistence.Write.Repositories;

internal sealed class CityRepository(ApplicationDbContext context) 
    : RenStore.Delivery.Domain.Interfaces.ICityRepository
{
    private readonly ApplicationDbContext _context = context 
                                                     ?? throw new ArgumentNullException(nameof(context));
    
    public async Task<int> AddAsync(
        City city, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(city);
        
        var result = await this._context.Cities.AddAsync(city, cancellationToken);
        
        return result.Entity.Id;
    }

    public async Task AddRangeAsync(
        IReadOnlyCollection<City> cities, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(cities);

        var citiesList = cities as IList<City> ?? cities.ToList();

        if (citiesList.Count == 0) 
            return;

        await this._context.Cities.AddRangeAsync(citiesList, cancellationToken);
    }

    public void Remove(City city)
    {
        ArgumentNullException.ThrowIfNull(city);

        this._context.Cities.Remove(city);
    }

    public void RemoveRange(IReadOnlyCollection<City> cities)
    {
        ArgumentNullException.ThrowIfNull(cities);
        
        var citiesList = cities as IList<City> ?? cities.ToList();

        if (citiesList.Count == 0) 
            return;
        
        this._context.Cities.RemoveRange(cities);
    }
}