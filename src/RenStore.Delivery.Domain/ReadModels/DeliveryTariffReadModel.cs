using RenStore.Delivery.Domain.Enums;

namespace RenStore.Delivery.Domain.ReadModels;

public sealed class DeliveryTariffReadModel
{
    public int                 Id            { get; set; }
    public decimal             PriceAmount   { get; set; }
    public string              Currency      { get; set; } = string.Empty;
    public decimal             WeightLimitKg { get; set; }
    public DeliveryTariffType  Type          { get; set; }
    public string              Description   { get; set; } = string.Empty;
    public bool                IsDeleted     { get; set; }
    public DateTimeOffset      CreatedAt     { get; set; }
    public DateTimeOffset?     UpdatedAt     { get; set; }
    public DateTimeOffset?     DeletedAt     { get; set; }
}