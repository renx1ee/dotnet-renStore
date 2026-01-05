using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Domain.Interfaces;

/// <summary>
///  Repository for working with <see cref="DeliveryOrder"/>.
///  Provide basic (CREATE REMOVE) operations.  
/// </summary>
public interface IDeliveryOrderRepository
{
    /// <summary>
    /// Create a new delivery order in the database.
    /// </summary>
    /// <param name="deliveryOrder">The delivery order entity to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <exception cref="ArgumentNullException">If delivery order is null.</exception>
    /// <returns>Delivery order unique identifier (ID) from the database.</returns>
    Task<Guid> AddAsync(
        DeliveryOrder deliveryOrder,
        CancellationToken cancellationToken);

    /// <summary>
    /// Create a range of new delivery orders in the database.
    /// </summary>
    /// <param name="deliveryOrders">Collection of delivery order entities to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <exception cref="ArgumentNullException">If collection of delivery orders is null.</exception>
    Task AddRangeAsync(
        IReadOnlyCollection<DeliveryOrder> deliveryOrders,
        CancellationToken cancellationToken);

    /// <summary>
    /// Remove delivery order entity from the database.
    /// </summary>
    /// <param name="deliveryOrder">Delivery order entity to remove.</param>
    /// <exception cref="ArgumentNullException">If delivery order is null.</exception>
    void Remove(DeliveryOrder deliveryOrder);

    /// <summary>
    /// Remove range of delivery orders from the database.
    /// </summary>
    /// <param name="deliveryOrderies">Collection of delivery order entities to remove.</param>
    /// <exception cref="ArgumentNullException">If collection of delivery order is null.</exception>
    void RemoveRange(IReadOnlyCollection<DeliveryOrder> deliveryOrderies);
}