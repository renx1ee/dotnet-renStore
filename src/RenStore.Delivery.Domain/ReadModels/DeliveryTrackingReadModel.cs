using RenStore.Delivery.Domain.Enums;

namespace RenStore.Delivery.Domain.ReadModels;

public sealed class DeliveryTrackingReadModel
{
    public Guid            Id              { get; set; }
    public Guid            DeliveryOrderId { get; set; }
    public DeliveryStatus  Status          { get; set; }
    public string          CurrentLocation { get; set; } = string.Empty;
    public string          Notes           { get; set; } = string.Empty;
    public long?           SortingCenterId { get; set; }
    public long?           PickupPointId   { get; set; }
    public DateTimeOffset  OccurredAt      { get; set; }
}