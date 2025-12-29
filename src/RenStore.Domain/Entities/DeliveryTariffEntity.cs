using RenStore.Domain.Enums;

namespace RenStore.Domain.Entities;

public class DeliveryTariffEntity
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public DeliveryTariffType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public double WeightLimitKg { get; set; }
    public IEnumerable<DeliveryOrderEntity>? DeliveryOrders { get; set; }
}
