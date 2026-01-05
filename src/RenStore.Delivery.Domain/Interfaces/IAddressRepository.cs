using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Domain.Interfaces;

/// <summary>
///  Repository for working with <see cref="Address"/>.
///  Provide basic (CREATE REMOVE) operations.  
/// </summary>
public interface IAddressRepository
{
    /// <summary>
    /// Create a new address in the database.
    /// </summary>
    /// <param name="address">The address entity to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <exception cref="ArgumentNullException">If address is null.</exception>
    /// <returns>Address unique identifier (ID) from the database.</returns>
    Task<Guid> AddAsync(
        Address address,
        CancellationToken cancellationToken);
    
    /// <summary>
    /// Create a range of new addresses in the database.
    /// </summary>
    /// <param name="addresses">Collection of address entities to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <exception cref="ArgumentNullException">If collection of addresses is null.</exception>
    Task AddRangeAsync(
        IReadOnlyCollection<Address> addresses,
        CancellationToken cancellationToken);
    
    /// <summary>
    /// Remove an address entity from the database.
    /// </summary>
    /// <param name="address">Address entity to remove.</param>
    /// <exception cref="ArgumentNullException">If address is null.</exception>
    void Remove(Address address);

    /// <summary>
    /// Remove range of addresses from the database.
    /// </summary>
    /// <param name="addressEntity">Collection of address entities to remove.</param>
    /// <exception cref="ArgumentNullException">If collection of addresses is null.</exception>
    void RemoveRange(IReadOnlyCollection<Address> addressEntity);
}