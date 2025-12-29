using RenStore.Domain.Enums;

namespace RenStore.Domain.Entities;

public class DeliveryTrackingEntity
{
    public Guid Id { get; set; }
    public string CurrentLocation { get; set; } = string.Empty;
    public DeliverTrackingStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Notes { get; set; } = string.Empty;
    public Guid DeliveryOrderId { get; set; }
    public DeliveryOrderEntity? DeliveryOrder { get; set; }
}

