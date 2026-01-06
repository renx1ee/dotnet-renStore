using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Domain.Interfaces;

/// <summary>
///  Repository for working with <see cref="SortingCenter"/>.
///  Provide basic (CREATE REMOVE) operations.  
/// </summary>
public interface ISortingCenterRepository
{
    /// <summary>
    /// Create a new <see cref="SortingCenter"/> in the database.
    /// </summary>
    /// <param name="sortingCenter">The sorting center entity to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <exception cref="ArgumentNullException">If sorting center is null.</exception>
    /// <returns>Sorting center unique identifier (ID) from the database.</returns>
    Task<long> CreateAsync(
        SortingCenter sortingCenter,
        CancellationToken cancellationToken);

    /// <summary>
    /// Create a range of new <see cref="SortingCenter"/> in the database.
    /// </summary>
    /// <param name="sortingCenters">Collection of sorting center entities to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <exception cref="ArgumentNullException">If collection of sorting centers is null.</exception>
    Task CreateRangeAsync(
        IReadOnlyCollection<SortingCenter> sortingCenters,
        CancellationToken cancellationToken);

    /// <summary>
    /// Remove a <see cref="SortingCenter"/> entity from the database.
    /// </summary>
    /// <param name="sortingCenter">Sorting center to remove.</param>
    /// <exception cref="ArgumentNullException">If sorting center is null.</exception>
    void Remove(SortingCenter sortingCenter);

    /// <summary>
    /// Remove range of <see cref="SortingCenter"/> from the database.
    /// </summary>
    /// <param name="sortingCenters">Collection of sorting center entities to remove.</param>
    /// <exception cref="ArgumentNullException">If collection of sorting centers is null.</exception>
    void RemoveRange(IReadOnlyCollection<SortingCenter> sortingCenters);
}