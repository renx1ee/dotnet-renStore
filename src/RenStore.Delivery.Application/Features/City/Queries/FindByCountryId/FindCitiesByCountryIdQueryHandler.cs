using RenStore.Delivery.Application.Abstractions.Queries;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.City.Queries.FindByCountryId;

internal sealed class FindCitiesByCountryIdQueryHandler(
    ICityQuery cityQuery,
    ILogger<FindCitiesByCountryIdQueryHandler> logger)
    : IRequestHandler<FindCitiesByCountryIdQuery, IReadOnlyList<CityReadModel>>
{
    public async Task<IReadOnlyList<CityReadModel>> Handle(
        FindCitiesByCountryIdQuery request,
        CancellationToken          cancellationToken)
    {
        logger.LogInformation(
            "Handling {Query}. CountryId={CountryId} IsDeleted={IsDeleted}",
            nameof(FindCitiesByCountryIdQuery),
            request.CountryId,
            request.IsDeleted);

        var result = await cityQuery.FindByCountryIdAsync(
            request.CountryId,
            request.IsDeleted,
            cancellationToken);

        logger.LogInformation(
            "Fetched {Count} cities for country. CountryId={CountryId}",
            result.Count, request.CountryId);

        return result;
    }
}