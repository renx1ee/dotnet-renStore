namespace RenStore.Delivery.Application.Features.DeliveryTariff.Commands.Create;

internal sealed class CreateDeliveryTariffCommandHandler(
    IDeliveryTariffRepository tariffRepository,
    ILogger<CreateDeliveryTariffCommandHandler> logger)
    : IRequestHandler<CreateDeliveryTariffCommand, int>
{
    public async Task<int> Handle(
        CreateDeliveryTariffCommand request,
        CancellationToken           cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. Type={Type} Price={Price}",
            nameof(CreateDeliveryTariffCommand), request.Type, request.PriceAmount);

        var tariff = Domain.Entities.DeliveryTariff.Create(
            priceAmount:   request.PriceAmount,
            currency:      request.Currency,
            weightLimitKg: request.WeightLimitKg,
            type:          request.Type,
            description:   request.Description,
            now:           DateTimeOffset.UtcNow);

        await tariffRepository.AddAsync(tariff, cancellationToken);
        await tariffRepository.CommitAsync(cancellationToken);

        logger.LogInformation(
            "DeliveryTariff created. TariffId={TariffId}", tariff.Id);

        return tariff.Id;
    }
}