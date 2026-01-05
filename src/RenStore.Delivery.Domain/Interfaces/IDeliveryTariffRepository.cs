using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Domain.Interfaces;

/// <summary>
///  Repository for working with <see cref="DeliveryTariff"/>.
///  Provide basic (CREATE REMOVE) operations.  
/// </summary>
public interface IDeliveryTariffRepository
{
    /// <summary>
    /// Create a new delivery tariff in the database.
    /// </summary>
    /// <param name="tariff">The delivery tariff entity to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <exception cref="ArgumentNullException">If delivery tariff is null.</exception>
    /// <returns>Delivery tariff unique identifier (ID) from the database.</returns>
    Task<Guid> AddAsync(
        DeliveryTariff tariff,
        CancellationToken cancellationToken);

    /// <summary>
    /// Create a range of new delivery tariffs in the database.
    /// </summary>
    /// <param name="tariffs">Collection of delivery tariff entities to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <exception cref="ArgumentNullException">If collection of delivery tariff is null.</exception>
    Task AddRangeAsync(
        IReadOnlyCollection<DeliveryTariff> tariffs,
        CancellationToken cancellationToken);

    /// <summary>
    /// Remove a delivery tariff entity from the database.
    /// </summary>
    /// <param name="tariff">Delivery tariff entity to remove.</param>
    /// <exception cref="ArgumentNullException">If delivery tariff is null.</exception>
    void Remove(DeliveryTariff tariff);

    /// <summary>
    /// Remove range of delivery tariffs from the database.
    /// </summary>
    /// <param name="tariffs">Collection of delivery tariff entities to remove.</param>
    /// <exception cref="ArgumentNullException">If collection of delivery tariffs is null.</exception>
    void Remove(IReadOnlyCollection<DeliveryTariff> tariffs);
}