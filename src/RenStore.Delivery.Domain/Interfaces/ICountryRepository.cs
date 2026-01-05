using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Domain.Interfaces;

/// <summary>
/// Repository for working with <see cref="Country"/>.
/// Provide basic (CREATE REMOVE) operations.
/// </summary>
public interface ICountryRepository
{
    /// <summary>
    /// Create a new country in the database.
    /// </summary>
    /// <param name="country">The country entity to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Country unique identifier (ID) from the database.</returns>
    
    Task<int> AddAsync(
        Country country,
        CancellationToken cancellationToken);
    /// <summary>
    /// Create a range of countries in the database.
    /// </summary>
    /// <param name="countries">Collection of countries to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
   
    Task AddRangeAsync(
        IReadOnlyCollection<Country> countries,
        CancellationToken cancellationToken);
    /// <summary>
    /// Remove a country from the database.
    /// </summary>
    /// <param name="country">country entity to remove.</param>
   
    void Remove(Country country);
    /// <summary>
    /// Remove range of countries from the database.
    /// </summary>
    /// <param name="countries">Collection of country entities.</param>
    void RemoveRange(IReadOnlyCollection<Country> countries);
}