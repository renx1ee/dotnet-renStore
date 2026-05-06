using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Application.Features.DeliveryTariff.Commands.Change;

internal sealed class ChangeTariffPriceCommandHandler(
    IDeliveryTariffRepository tariffRepository,
    ILogger<ChangeTariffPriceCommandHandler> logger)
    : IRequestHandler<ChangeTariffPriceCommand>
{
    public async Task Handle(
        ChangeTariffPriceCommand request,
        CancellationToken        cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. TariffId={TariffId} NewPrice={Price}",
            nameof(ChangeTariffPriceCommand), request.TariffId, request.PriceAmount);

        var tariff = await tariffRepository.GetAsync(request.TariffId, cancellationToken);

        if (tariff is null)
            throw new NotFoundException(name: typeof(Domain.Entities.DeliveryTariff), request.TariffId);
        
        tariff.ChangePrice(
            now: DateTimeOffset.UtcNow,
            priceAmount: request.PriceAmount,
            currency: request.Currency);

        await tariffRepository.CommitAsync(cancellationToken);

        logger.LogInformation("Tariff price changed. TariffId={TariffId}", request.TariffId);
    }
}