namespace RenStore.Delivery.Application.Features.City.Commands.Create;

internal sealed class CreateCityCommandHandler(
    ICityRepository cityRepository,
    ILogger<CreateCityCommandHandler> logger)
    : IRequestHandler<CreateCityCommand, int>
{
    public async Task<int> Handle(
        CreateCityCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. Name={Name} CountryId={CountryId}",
            nameof(CreateCityCommand), request.Name, request.CountryId);

        var city = Domain.Entities.City.Create(
            name:      request.Name,
            nameRu:    request.NameRu,
            countryId: request.CountryId,
            now:       DateTimeOffset.UtcNow);

        await cityRepository.AddAsync(city, cancellationToken);
        await cityRepository.CommitAsync(cancellationToken);

        logger.LogInformation("City created. CityId={CityId} Name={Name}", city.Id, city.Name);

        return city.Id;
    }
}