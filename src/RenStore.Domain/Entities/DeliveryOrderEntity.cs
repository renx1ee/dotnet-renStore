namespace RenStore.Domain.Entities;

public class DeliveryOrderEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeliveredAt { get; set; }
    public Guid OrderId { get; set; }
    public OrderEntity? Order { get; set; }
    public Guid AddressId { get; set; }
    public AddressEntity? Address { get; set; }
    public Guid DeliveryTariffId { get; set; }
    public DeliveryTariffEntity? DeliveryTariff { get; set; }
    public IEnumerable<DeliveryTrackingEntity>? TrackingHistory { get; set; }
}