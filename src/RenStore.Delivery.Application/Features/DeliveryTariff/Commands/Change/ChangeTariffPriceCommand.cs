namespace RenStore.Delivery.Application.Features.DeliveryTariff.Commands.Change;

public sealed record ChangeTariffPriceCommand(
    int     TariffId,
    decimal PriceAmount,
    string  Currency) : IRequest;