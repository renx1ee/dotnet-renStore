using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Domain.Interfaces;

/// <summary>
/// Repository for working with <see cref="City"/>.
/// Provide basic (CREATE REMOVE) operations.
/// </summary>
public interface ICityRepository
{
    /// <summary>
    /// Create a new city in the database.
    /// </summary>
    /// <param name="city">The city entity to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>City unique identifier (ID) from the database.</returns>
    Task<int> AddAsync(
        City city,
        CancellationToken cancellationToken);
   
    /// <summary>
    /// Create a range of cities in the database.
    /// </summary>
    /// <param name="cities">Collection of cities to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    Task AddRangeAsync(
        IReadOnlyCollection<City> cities,
        CancellationToken cancellationToken);
    
    /// <summary>
    /// Remove a city from the database.
    /// </summary>
    /// <param name="city">City entity to remove.</param>
    void Remove(City city);
    
    /// <summary>
    /// Remove range of cities from the database.
    /// </summary>
    /// <param name="cities">Collection of city entities.</param>
    void RemoveRange(IReadOnlyCollection<City> cities);
}