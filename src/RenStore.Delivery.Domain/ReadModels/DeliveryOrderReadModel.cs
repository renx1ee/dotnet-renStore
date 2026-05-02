using RenStore.Delivery.Domain.Enums;

namespace RenStore.Delivery.Domain.ReadModels;

public sealed class DeliveryOrderReadModel
{
    public Guid            Id                         { get; set; }
    public Guid            OrderId                    { get; set; }
    public int             DeliveryTariffId           { get; set; }
    public DeliveryStatus  Status                     { get; set; }
    public long?           CurrentSortingCenterId     { get; set; }
    public long?           DestinationSortingCenterId { get; set; }
    public long?           PickupPointId              { get; set; }
    public DateTimeOffset  CreatedAt                  { get; set; }
    public DateTimeOffset? DeliveredAt                { get; set; }
    public DateTimeOffset? DeletedAt                  { get; set; }
}