using RenStore.Delivery.Domain.Enums;

namespace RenStore.Delivery.WebApi.Requests;

public sealed record CreateDeliveryTariffRequest(
    decimal            PriceAmount,
    string             Currency,
    decimal            WeightLimitKg,
    DeliveryTariffType Type,
    string             Description);