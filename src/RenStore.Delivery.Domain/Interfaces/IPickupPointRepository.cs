using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Domain.Interfaces;

/// <summary>
///  Repository for working with <see cref="PickupPoint"/>.
///  Provide basic (CREATE REMOVE) operations.  
/// </summary>
public interface IPickupPointRepository
{
    /// <summary>
    /// Create a new <see cref="PickupPoint"/> in the database.
    /// </summary>
    /// <param name="pickupPoint">The pickup point entity to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <exception cref="ArgumentNullException">If pickup point is null.</exception>
    /// <returns>Pickup point unique identifier (ID) from the database.</returns>
    Task<long> CreateAsync(
        PickupPoint pickupPoint,
        CancellationToken cancellationToken);

    /// <summary>
    /// Create a range of new <see cref="PickupPoint"/> in the database.
    /// </summary>
    /// <param name="pickupPoints">Collection of pickup point entities to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <exception cref="ArgumentNullException">If collection of pickup points is null.</exception>
    Task CreateRangeAsync(
        IReadOnlyCollection<PickupPoint> pickupPoints,
        CancellationToken cancellationToken);

    /// <summary>
    /// Remove a <see cref="PickupPoint"/> entity from the database.
    /// </summary>
    /// <param name="pickupPoint">Pickup pointentity to remove.</param>
    /// <exception cref="ArgumentNullException">If pickup point is null.</exception>
    void Remove(PickupPoint pickupPoint);

    /// <summary>
    /// Remove range of <see cref="pickupPoints"/> from the database.
    /// </summary>
    /// <param name="pickupPoints">Collection of pickup point entities to remove.</param>
    /// <exception cref="ArgumentNullException">If collection of pickup points is null.</exception>
    void RemoveRange(IReadOnlyCollection<PickupPoint> pickupPoints);
}