namespace RenStore.Delivery.Domain.Entities;

public class DeliveryOrder
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeliveredAt { get; set; }
    public Guid OrderId { get; set; }
    /*public OrderEntity? Order { get; set; }*/
    public Guid AddressId { get; set; }
    public Address? Address { get; set; }
    public Guid DeliveryTariffId { get; set; }
    public DeliveryTariff? DeliveryTariff { get; set; }
    public IEnumerable<DeliveryTracking>? TrackingHistory { get; set; }
}