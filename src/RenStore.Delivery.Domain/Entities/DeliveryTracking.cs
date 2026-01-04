using RenStore.Delivery.Domain.Enums;

namespace RenStore.Delivery.Domain.Entities;

public class DeliveryTracking
{
    public Guid Id { get; }
    public string CurrentLocation { get; } = string.Empty;
    public DeliveryStatus Status { get; }
    public DateTimeOffset OccurredAt { get; }
    public string Notes { get; } = string.Empty;
    public long? SortingCenterId { get; }
    public Guid DeliveryOrderId { get; }

    public DeliveryTracking(
        string? currentLocation,
        DeliveryStatus status,
        DateTimeOffset occurredAt,
        string? notes,
        Guid deliveryOrderId,
        long? sortingCenterId = null)
    {
        if(!string.IsNullOrEmpty(currentLocation))
            CurrentLocation = currentLocation;
        
        if(!string.IsNullOrEmpty(notes))
            Notes = notes;
        
        if (sortingCenterId != null &&
            sortingCenterId != 0)
        {
            SortingCenterId = sortingCenterId;
        }
        
        Status = status;
        OccurredAt = occurredAt;
        DeliveryOrderId = deliveryOrderId;
    }
}

