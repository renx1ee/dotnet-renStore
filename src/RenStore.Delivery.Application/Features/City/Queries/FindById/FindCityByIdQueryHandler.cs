using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.City.Queries.FindById;

internal sealed class FindCityByIdQueryHandler(
    ICityQuery cityQuery,
    ILogger<FindCityByIdQueryHandler> logger)
    : IRequestHandler<FindCityByIdQuery, CityReadModel?>
{
    public async Task<CityReadModel?> Handle(
        FindCityByIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handling {Query}. CityId={CityId}",
            nameof(FindCityByIdQuery), request.CityId);

        var result = await cityQuery.FindByIdAsync(request.CityId, cancellationToken);

        if (result is null)
            logger.LogWarning("City not found. CityId={CityId}", request.CityId);

        return result;
    }
}