using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.Country.Queries.FindById;

internal sealed class FindCountryByIdQueryHandler(
    ICountryQuery countryQuery,
    ILogger<FindCountryByIdQueryHandler> logger)
    : IRequestHandler<FindCountryByIdQuery, CountryReadModel?>
{
    public async Task<CountryReadModel?> Handle(
        FindCountryByIdQuery request,
        CancellationToken    cancellationToken)
    {
        logger.LogInformation(
            "Handling {Query}. CountryId={CountryId}",
            nameof(FindCountryByIdQuery), request.CountryId);

        var result = await countryQuery.FindByIdAsync(request.CountryId, cancellationToken);

        if (result is null)
            logger.LogWarning("Country not found. CountryId={CountryId}", request.CountryId);

        return result;
    }
}