namespace RenStore.Delivery.Application.Features.DeliveryTariff.Commands.Delete;

public sealed record DeleteDeliveryTariffCommand(int TariffId) : IRequest;