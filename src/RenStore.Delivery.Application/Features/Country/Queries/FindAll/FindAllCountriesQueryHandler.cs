using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.Country.Queries.FindAll;

internal sealed class FindAllCountriesQueryHandler(
    ICountryQuery countryQuery,
    ILogger<FindAllCountriesQueryHandler> logger)
    : IRequestHandler<FindAllCountriesQuery, IReadOnlyList<CountryReadModel>>
{
    public async Task<IReadOnlyList<CountryReadModel>> Handle(
        FindAllCountriesQuery request,
        CancellationToken     cancellationToken)
    {
        logger.LogInformation(
            "Handling {Query}. IsDeleted={IsDeleted}",
            nameof(FindAllCountriesQuery), request.IsDeleted);

        var result = await countryQuery.FindAllAsync(request.IsDeleted, cancellationToken);

        logger.LogInformation("Fetched {Count} countries.", result.Count);

        return result;
    }
}