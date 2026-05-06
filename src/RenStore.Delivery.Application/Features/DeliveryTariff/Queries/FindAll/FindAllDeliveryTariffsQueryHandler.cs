using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.DeliveryTariff.Queries.FindAll;

internal sealed class FindAllDeliveryTariffsQueryHandler(
    IDeliveryTariffQuery tariffQuery,
    ILogger<FindAllDeliveryTariffsQueryHandler> logger)
    : IRequestHandler<FindAllDeliveryTariffsQuery, IReadOnlyList<DeliveryTariffReadModel>>
{
    public async Task<IReadOnlyList<DeliveryTariffReadModel>> Handle(
        FindAllDeliveryTariffsQuery request,
        CancellationToken           cancellationToken)
    {
        logger.LogInformation(
            "Handling {Query}. IsDeleted={IsDeleted}",
            nameof(FindAllDeliveryTariffsQuery), request.IsDeleted);

        var result = await tariffQuery.FindAllAsync(request.IsDeleted, cancellationToken);

        logger.LogInformation("Fetched {Count} tariffs.", result.Count);

        return result;
    }
}