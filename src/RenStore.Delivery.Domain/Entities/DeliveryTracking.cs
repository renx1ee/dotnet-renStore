using RenStore.Delivery.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Domain.Entities;

public class DeliveryTracking
{
    public Guid Id { get; private set; }
    public string CurrentLocation { get; private set; } = string.Empty;
    public DeliveryStatus Status { get; private set; }
    public string Notes { get; private set; } = string.Empty;
    public bool IsDeleted { get; private set; }
    public DateTimeOffset OccurredAt { get; private set; }
    public DateTimeOffset DeletedAt { get; private set; }
    public long? SortingCenterId { get; private set; }
    private SortingCenter _sortingCenter { get;  }
    public Guid DeliveryOrderId { get; private set; }
    private DeliveryOrder _deliveryOrder { get; }
    
    private DeliveryTracking() { }

    public static DeliveryTracking Create(
        string? currentLocation,
        DeliveryStatus status,
        DateTimeOffset occurredAt,
        string? notes,
        Guid deliveryOrderId,
        long? sortingCenterId = null)
    {
        var tracking = new DeliveryTracking()
        {       
            Id = Guid.NewGuid(),
            Status = status,
            OccurredAt = occurredAt,
            DeliveryOrderId = deliveryOrderId,
        };
        
        if(!string.IsNullOrEmpty(currentLocation))
            tracking.CurrentLocation = currentLocation;
        
        if(!string.IsNullOrEmpty(notes))
            tracking.Notes = notes;
        
        if (sortingCenterId != null &&
            sortingCenterId != 0)
        {
            tracking.SortingCenterId = sortingCenterId;
        }

        return tracking;
    }

    public void Delete(DateTimeOffset now)
    {
        if (IsDeleted)
            throw new DomainException("Cannot delete already deleted entity.");
        
        IsDeleted = true;
        DeletedAt = now;
    }
}

