using RenStore.Delivery.Domain.Enums;

namespace RenStore.Delivery.Application.Features.DeliveryTariff.Commands.Create;

public sealed record CreateDeliveryTariffCommand(
    decimal            PriceAmount,
    string             Currency,
    decimal            WeightLimitKg,
    DeliveryTariffType Type,
    string             Description) : IRequest<int>;