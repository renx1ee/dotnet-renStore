using RenStore.Delivery.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Domain.Entities;

/// <summary>
/// Represents a delivery order physical entity with life cycle and invariants.
/// </summary>
public class DeliveryOrder
{
    private readonly List<DeliveryTracking> _trackingHistory = new();
    
    public Guid Id { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? DeliveredAt { get; private set; } = null;
    public DateTimeOffset? DeletedAt { get; private set; } = null;
    public DeliveryStatus Status { get; private set; }
    public Guid OrderId { get; private set; } // TODO:
    public Guid DeliveryTariffId { get; private set; }
    private DeliveryTariff _tariff { get; }
    public long? CurrentSortingCenterId { get; private set; }
    private SortingCenter? _currentSortingCenter { get; }
    public long? DestinationSortingCenterId { get; private set; } // TODO: не добавлен в конфиг как FK
    private SortingCenter? _destinationSortingCenter { get; } // TODO: не добавлен в конфиг как FK
    public long? PickupPointId { get; private set; }
    private PickupPoint _pickupPoint { get; }
    public IReadOnlyCollection<DeliveryTracking> TrackingHistory => _trackingHistory;
    
    private DeliveryOrder() { }
    
    /// <summary>
    /// Create a new Delivery Order insuring all invariants are satisfied.
    /// </summary>
    /// <exception cref="DomainException">if the Delivery order parameters are null or empty, or any IDs are less 0.</exception>
    public static DeliveryOrder Create(
        Guid orderId,
        Guid deliveryTariffId,
        DateTimeOffset now)
    {
        if (orderId == Guid.Empty)
            throw new DomainException("Order ID cannot be empty.");
        
        if (deliveryTariffId == Guid.Empty)
            throw new DomainException("Delivery Tariff ID cannot be empty.");
        
        var order = new DeliveryOrder()
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            DeliveryTariffId = deliveryTariffId,
            Status = DeliveryStatus.Placed
        };
        
        order.AddTracking(status: DeliveryStatus.Placed, now: now);

        return order;
    }
    
    /// <summary>
    /// На сборке продавцом.
    /// </summary>
    /// <param name="now"></param>
    /// <exception cref="DomainException"></exception>
    public void MarkAsAssemblingBySeller(DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        if (Status != DeliveryStatus.Placed)
            throw new DomainException("Cannot mark delivery as be assembling");
        
        UpdateStatus(newStatus: DeliveryStatus.AssemblingBySeller, now: now);
    }
    
    /// <summary>
    /// Отправлен в сортировочный центр.
    /// </summary>
    /// <param name="now"></param>
    /// <param name="toSortingCenterId"></param>
    /// <exception cref="DomainException"></exception>
    public void ShipToSortingCenter(long toSortingCenterId, DateTimeOffset now)
    {
        if (Status != DeliveryStatus.AssemblingBySeller ||
            Status != DeliveryStatus.Sorted)
            throw new DomainException("Delivery cannot be send to sorting center.");

        DestinationSortingCenterId = toSortingCenterId;
        CurrentSortingCenterId = null;
        
        UpdateStatus(
            newStatus: DeliveryStatus.EnRouteToSortingCenter, 
            sortingCenter: toSortingCenterId,
            now: now);
    }
    
    /// <summary>
    /// Прибыл в сортировочный центр.
    /// </summary>
    public void MarkAsArrivedAtSortingCenter(long sortingCenterId, DateTimeOffset now)
    {
        if (Status != DeliveryStatus.EnRouteToSortingCenter) 
            throw new DomainException("Delivery cannot be send to sorting center.");

        if (DestinationSortingCenterId != sortingCenterId)
            throw new DomainException("Wrong sorting center");

        DestinationSortingCenterId = null;
        CurrentSortingCenterId = sortingCenterId;
        
        UpdateStatus(
            newStatus: DeliveryStatus.ArrivedAtSortingCenter, 
            sortingCenter: sortingCenterId,
            now: now);
    }
    
    /// <summary>
    /// Отсортирован.
    /// </summary>
    /// <param name="sortingCenterId"></param>
    /// <param name="now"></param>
    /// <exception cref="DomainException"></exception>
    public void SortAtSortingCenter(
        long sortingCenterId,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        if (Status != DeliveryStatus.ArrivedAtSortingCenter) 
            throw new DomainException("Cannot mark delivery as sorted.");

        if (CurrentSortingCenterId != sortingCenterId)
            throw new DomainException("Wrong sorting center.");
        
        UpdateStatus(
            newStatus: DeliveryStatus.Sorted, 
            sortingCenter: sortingCenterId,
            now: now);
    }
    
    /// <summary>
    /// В пути в пункт выдачи.
    /// </summary>
    /// <param name="pickupPoint"></param>
    /// <param name="now"></param>
    /// <exception cref="DomainException"></exception>
    public void ShipToPickupPoint(long pickupPoint, DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        if (Status != DeliveryStatus.Sorted)
            throw new DomainException("Cannot mark delivery as en route to pickup point.");

        PickupPointId = pickupPoint;
        
        UpdateStatus(newStatus: DeliveryStatus.EnRouteToPickupPoint, now: now);
    }
    
    /// <summary>
    /// Прибыл в пункт выдачи.
    /// </summary>
    /// <param name="now"></param>
    /// <exception cref="DomainException"></exception>
    public void MarkAsAwaitingPickup(DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        if (Status != DeliveryStatus.EnRouteToPickupPoint)
            throw new DomainException("Cannot mark delivery as awaiting pickup.");
        
        UpdateStatus(newStatus: DeliveryStatus.AwaitingPickup, now: now);
    }
    
    /// <summary>
    /// Покупатель забрал товар.
    /// </summary>
    /// <param name="now"></param>
    /// <exception cref="DomainException"></exception>
    public void MarkAsDelivered(DateTimeOffset now)
    {
        EnsureNotDeleted("Cannot deliver already deleted delivery order.");
        
        if (Status == DeliveryStatus.Delivered)
            throw new DomainException("Cannot deliver already delivered delivery order.");
        
        if (Status != DeliveryStatus.AwaitingPickup)
            throw new DomainException("Cannot mark delivery as Delivered.");
        
        UpdateStatus(newStatus: DeliveryStatus.Delivered, now: now);
        DeliveredAt = now;
    }

    /// <summary>
    /// Товар отправлен продавцу.
    /// </summary>
    /// <param name="now"></param>
    public void Return(DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        // TODO: сделать проверку, можно ли вернуть товар
        
        UpdateStatus(newStatus: DeliveryStatus.Returned, now: now);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="now"></param>
    /// <exception cref="DomainException"></exception>
    public void Delete(DateTimeOffset now)
    {
        if (Status == DeliveryStatus.IsDeleted)
            throw new DomainException("Cannot deleted already deleted delivery order.");
        
        UpdateStatus(newStatus: DeliveryStatus.IsDeleted, now: now);
        
        DeletedAt = now;
    }
    
    private void EnsureNotDeleted(string? message = null)
    {
        if(Status == DeliveryStatus.IsDeleted)
            throw new DomainException(message ?? "Delivery order already deleted.");
    }
    
    private void AddTracking(
        DeliveryStatus status, 
        DateTimeOffset now, 
        long? sortingCenter = null, 
        string? location = null)
    {
        _trackingHistory.Add(
            DeliveryTracking.Create(
                currentLocation: location ?? string.Empty,
                notes: string.Empty,
                status: status,
                occurredAt: now,
                deliveryOrderId: Id,
                sortingCenterId: sortingCenter));
    }

    private void UpdateStatus(
        DeliveryStatus newStatus, 
        DateTimeOffset now, 
        long? sortingCenter = null, 
        string? location = null)
    {
        Status = newStatus;
        
        this.AddTracking(
            status: newStatus, 
            now: now, 
            location: location, 
            sortingCenter: sortingCenter);
    }
}