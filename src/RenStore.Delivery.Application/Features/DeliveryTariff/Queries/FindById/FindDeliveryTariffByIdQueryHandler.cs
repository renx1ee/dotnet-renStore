using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.DeliveryTariff.Queries.FindById;

internal sealed class FindDeliveryTariffByIdQueryHandler(
    IDeliveryTariffQuery tariffQuery,
    ILogger<FindDeliveryTariffByIdQueryHandler> logger)
    : IRequestHandler<FindDeliveryTariffByIdQuery, DeliveryTariffReadModel?>
{
    public async Task<DeliveryTariffReadModel?> Handle(
        FindDeliveryTariffByIdQuery request,
        CancellationToken           cancellationToken)
    {
        logger.LogInformation(
            "Handling {Query}. TariffId={TariffId}",
            nameof(FindDeliveryTariffByIdQuery), request.TariffId);

        var result = await tariffQuery.FindByIdAsync(request.TariffId, cancellationToken);

        if (result is null)
            logger.LogWarning("Tariff not found. TariffId={TariffId}", request.TariffId);

        return result;
    }
}