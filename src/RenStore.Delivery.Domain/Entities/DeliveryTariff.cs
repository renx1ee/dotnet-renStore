using RenStore.Delivery.Domain.Enums;

namespace RenStore.Delivery.Domain.Entities;

public class DeliveryTariff
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public DeliveryTariffType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public double WeightLimitKg { get; set; }
    public Guid DeliveryOrderId { get; set; }
    public DeliveryOrder? DeliveryOrder { get; set; }
}
