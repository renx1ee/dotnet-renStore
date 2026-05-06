// Domain/Aggregates/DeliveryOrder/DeliveryOrder.cs

using RenStore.Delivery.Domain.Aggregates.DeliveryOrder.Events;
using RenStore.Delivery.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Domain.Aggregates.DeliveryOrder;

/// <summary>
/// Represents a delivery order aggregate with event sourcing.
/// </summary>
public sealed class DeliveryOrder : AggregateRoot
{
    private readonly List<DeliveryTrackingSnapshot> _trackingHistory = new();

    public Guid           Id                         { get; private set; }
    public DeliveryStatus Status                     { get; private set; }
    public Guid           OrderId                    { get; private set; }
    public int            DeliveryTariffId           { get; private set; }
    public long?          CurrentSortingCenterId     { get; private set; }
    public long?          DestinationSortingCenterId { get; private set; }
    public long?          PickupPointId              { get; private set; }
    public string?        TrackingNumber             { get; set; }
    public DateTimeOffset CreatedAt                  { get; private set; }
    public DateTimeOffset? DeliveredAt               { get; private set; }
    public DateTimeOffset? DeletedAt                 { get; private set; }

    public IReadOnlyCollection<DeliveryTrackingSnapshot> TrackingHistory
        => _trackingHistory.AsReadOnly();

    private DeliveryOrder() { }

    public static DeliveryOrder Create(
        Guid           orderId,
        int            deliveryTariffId,
        DateTimeOffset now)
    {
        if (orderId == Guid.Empty)
            throw new DomainException("Order ID cannot be empty.");

        if (deliveryTariffId <= 0)
            throw new DomainException("Delivery Tariff ID cannot be less than 1.");

        var order = new DeliveryOrder();

        order.Raise(new DeliveryOrderCreatedEvent(
            EventId:         Guid.NewGuid(),
            OccurredAt:      now,
            DeliveryOrderId: Guid.NewGuid(),
            OrderId:         orderId,
            DeliveryTariffId: deliveryTariffId));

        return order;
    }

    public void MarkAsAssemblingBySeller(DateTimeOffset now)
    {
        EnsureNotDeleted();

        if (Status != DeliveryStatus.Placed)
            throw new DomainException("Cannot mark delivery as assembling — must be in Placed status.");

        Raise(new DeliveryOrderAssemblingBySellerEvent(
            EventId:         Guid.NewGuid(),
            OccurredAt:      now,
            DeliveryOrderId: Id));
    }

    public void ShipToSortingCenter(long toSortingCenterId, DateTimeOffset now)
    {
        EnsureNotDeleted();

        if (Status != DeliveryStatus.AssemblingBySeller &&
            Status != DeliveryStatus.Sorted)
            throw new DomainException("Cannot ship to sorting center from current status.");

        if (toSortingCenterId <= 0)
            throw new DomainException("Sorting Center Id cannot be 0 or less.");

        Raise(new DeliveryOrderShippedToSortingCenterEvent(
            EventId:                   Guid.NewGuid(),
            OccurredAt:                now,
            DeliveryOrderId:           Id,
            DestinationSortingCenterId: toSortingCenterId));
    }

    public void MarkAsArrivedAtSortingCenter(long sortingCenterId, DateTimeOffset now)
    {
        EnsureNotDeleted();

        if (Status != DeliveryStatus.EnRouteToSortingCenter)
            throw new DomainException("Cannot arrive at sorting center from current status.");

        if (DestinationSortingCenterId != sortingCenterId)
            throw new DomainException("Wrong sorting center.");

        Raise(new DeliveryOrderArrivedAtSortingCenterEvent(
            EventId:         Guid.NewGuid(),
            OccurredAt:      now,
            DeliveryOrderId: Id,
            SortingCenterId: sortingCenterId));
    }

    public void SortAtSortingCenter(long sortingCenterId, DateTimeOffset now)
    {
        EnsureNotDeleted();

        if (Status != DeliveryStatus.ArrivedAtSortingCenter)
            throw new DomainException("Cannot sort from current status.");

        if (CurrentSortingCenterId != sortingCenterId)
            throw new DomainException("Wrong sorting center.");

        Raise(new DeliveryOrderSortedEvent(
            EventId:         Guid.NewGuid(),
            OccurredAt:      now,
            DeliveryOrderId: Id,
            SortingCenterId: sortingCenterId));
    }

    public void ShipToPickupPoint(long pickupPointId, DateTimeOffset now)
    {
        EnsureNotDeleted();

        if (Status != DeliveryStatus.Sorted)
            throw new DomainException("Cannot ship to pickup point — must be Sorted.");
        
        if (pickupPointId <= 0)
            throw new DomainException("Pickup Point Id cannot be 0 or less.");

        Raise(new DeliveryOrderShippedToPickupPointEvent(
            EventId:         Guid.NewGuid(),
            OccurredAt:      now,
            DeliveryOrderId: Id,
            PickupPointId:   pickupPointId));
    }

    public void MarkAsAwaitingPickup(long pickupPointId, DateTimeOffset now)
    {
        EnsureNotDeleted();

        if (Status != DeliveryStatus.EnRouteToPickupPoint)
            throw new DomainException("Cannot mark as awaiting pickup from current status.");

        if (PickupPointId != pickupPointId)
            throw new DomainException("Wrong pickup point.");

        Raise(new DeliveryOrderAwaitingPickupEvent(
            EventId:         Guid.NewGuid(),
            OccurredAt:      now,
            DeliveryOrderId: Id,
            PickupPointId:   pickupPointId));
    }

    public void AssignTrackingNumber(
        string trackingNumber,
        DateTimeOffset now)
    {
        EnsureNotDeleted();

        if (string.IsNullOrWhiteSpace(trackingNumber))
            throw new DomainException("Tracking number cannot be empty string.");

        Raise(new DeliveryOrderTrackingNumberAssignedEvent(
            EventId:         Guid.NewGuid(),
            OccurredAt:      now,
            DeliveryOrderId: Id,
            TrackingNumber:  trackingNumber));
    }
    
    public void MarkAsDelivered(DateTimeOffset now)
    {
        EnsureNotDeleted();

        if (Status != DeliveryStatus.AwaitingPickup)
            throw new DomainException("Cannot mark as delivered — must be AwaitingPickup.");

        Raise(new DeliveryOrderDeliveredEvent(
            EventId:         Guid.NewGuid(),
            OccurredAt:      now,
            DeliveryOrderId: Id));
    }

    public void Return(DateTimeOffset now)
    {
        EnsureNotDeleted();

        Raise(new DeliveryOrderReturnedEvent(
            EventId:         Guid.NewGuid(),
            OccurredAt:      now,
            DeliveryOrderId: Id));
    }

    public void Delete(DateTimeOffset now)
    {
        EnsureNotDeleted("Cannot delete already deleted delivery order.");

        Raise(new DeliveryOrderDeletedEvent(
            EventId:         Guid.NewGuid(),
            OccurredAt:      now,
            DeliveryOrderId: Id));
    }

    protected override void Apply(IDomainEvent @event)
    {
        switch (@event)
        {
            case DeliveryOrderCreatedEvent e:
                Id               = e.DeliveryOrderId;
                OrderId          = e.OrderId;
                DeliveryTariffId = e.DeliveryTariffId;
                Status           = DeliveryStatus.Placed;
                CreatedAt        = e.OccurredAt;
                AddTracking(DeliveryStatus.Placed, e.OccurredAt);
                break;

            case DeliveryOrderAssemblingBySellerEvent e:
                Status = DeliveryStatus.AssemblingBySeller;
                AddTracking(DeliveryStatus.AssemblingBySeller, e.OccurredAt);
                break;

            case DeliveryOrderShippedToSortingCenterEvent e:
                Status                     = DeliveryStatus.EnRouteToSortingCenter;
                DestinationSortingCenterId = e.DestinationSortingCenterId;
                CurrentSortingCenterId     = null;
                AddTracking(DeliveryStatus.EnRouteToSortingCenter, e.OccurredAt,
                    sortingCenterId: e.DestinationSortingCenterId);
                break;

            case DeliveryOrderArrivedAtSortingCenterEvent e:
                Status                     = DeliveryStatus.ArrivedAtSortingCenter;
                CurrentSortingCenterId     = e.SortingCenterId;
                DestinationSortingCenterId = null;
                AddTracking(DeliveryStatus.ArrivedAtSortingCenter, e.OccurredAt,
                    sortingCenterId: e.SortingCenterId);
                break;

            case DeliveryOrderSortedEvent e:
                Status = DeliveryStatus.Sorted;
                AddTracking(DeliveryStatus.Sorted, e.OccurredAt,
                    sortingCenterId: e.SortingCenterId);
                break;

            case DeliveryOrderShippedToPickupPointEvent e:
                Status        = DeliveryStatus.EnRouteToPickupPoint;
                PickupPointId = e.PickupPointId;
                AddTracking(DeliveryStatus.EnRouteToPickupPoint, e.OccurredAt,
                    pickupPointId: e.PickupPointId);
                break;

            case DeliveryOrderAwaitingPickupEvent e:
                Status = DeliveryStatus.AwaitingPickup;
                AddTracking(DeliveryStatus.AwaitingPickup, e.OccurredAt,
                    pickupPointId: e.PickupPointId);
                break;

            case DeliveryOrderDeliveredEvent e:
                Status      = DeliveryStatus.Delivered;
                DeliveredAt = e.OccurredAt;
                AddTracking(DeliveryStatus.Delivered, e.OccurredAt);
                break;

            case DeliveryOrderReturnedEvent e:
                Status = DeliveryStatus.Returned;
                AddTracking(DeliveryStatus.Returned, e.OccurredAt);
                break;

            case DeliveryOrderDeletedEvent e:
                Status    = DeliveryStatus.IsDeleted;
                DeletedAt = e.OccurredAt;
                AddTracking(DeliveryStatus.IsDeleted, e.OccurredAt);
                break;
        }
    }

    public static DeliveryOrder Rehydrate(IEnumerable<IDomainEvent> history)
    {
        var order = new DeliveryOrder();

        foreach (var @event in history)
        {
            order.Apply(@event);
            order.Version++;
        }

        return order;
    }

    private void EnsureNotDeleted(string? message = null)
    {
        if (Status == DeliveryStatus.IsDeleted)
            throw new DomainException(message ?? "Delivery order is deleted.");
    }

    private void AddTracking(
        DeliveryStatus status,
        DateTimeOffset now,
        long?          sortingCenterId = null,
        long?          pickupPointId   = null)
    {
        _trackingHistory.Add(new DeliveryTrackingSnapshot(
            Status:          status,
            OccurredAt:      now,
            SortingCenterId: sortingCenterId,
            PickupPointId:   pickupPointId));
    }
}

public sealed record DeliveryTrackingSnapshot(
    DeliveryStatus Status,
    DateTimeOffset OccurredAt,
    long?          SortingCenterId,
    long?          PickupPointId);