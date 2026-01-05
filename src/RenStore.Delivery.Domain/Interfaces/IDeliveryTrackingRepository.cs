using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Domain.Interfaces;

/// <summary>
///  Repository for working with <see cref="DeliveryTracking"/>.
///  Provide basic (CREATE REMOVE) operations.  
/// </summary>
public interface IDeliveryTrackingRepository
{
    /// <summary>
    /// Create a new delivery tracking in the database.
    /// </summary>
    /// <param name="tracking">The delivery tracking entity to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <exception cref="ArgumentNullException">If delivery tracking is null.</exception>
    /// <returns>Delivery tracking unique identifier (ID) from the database.</returns>
    Task<Guid> CreateAsync(
        DeliveryTracking tracking,
        CancellationToken cancellationToken);

    /// <summary>
    /// Create a range of new delivery trackings in the database.
    /// </summary>
    /// <param name="trackings">Collection of delivery tracking entities to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <exception cref="ArgumentNullException">If collection of delivery trackings is null.</exception>
    Task CreateRangeAsync(
        IReadOnlyList<DeliveryTracking> trackings,
        CancellationToken cancellationToken);

    /// <summary>
    /// Remove a delivery tracking entity from the database.
    /// </summary>
    /// <param name="tracking">delivery tracking entity to remove.</param>
    /// <exception cref="ArgumentNullException">If delivery tracking is null.</exception>
    void Remove(DeliveryTracking tracking);

    /// <summary>
    /// Remove range of delivery trackings from the database.
    /// </summary>
    /// <param name="trackings">Collection of delivery tracking entities to remove.</param>
    /// <exception cref="ArgumentNullException">If collection of delivery trackings is null.</exception>
    void RemoveRange(IReadOnlyList<DeliveryTracking> trackings);
}