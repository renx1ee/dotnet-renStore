namespace RenStore.Delivery.Domain.Entities;

public class DeliveryTariff
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    /*public DeliveryTariffType Type { get; set; }*/
    public string Description { get; set; } = string.Empty;
    public double WeightLimitKg { get; set; }
    public IEnumerable<DeliveryOrder>? DeliveryOrders { get; set; }
}
