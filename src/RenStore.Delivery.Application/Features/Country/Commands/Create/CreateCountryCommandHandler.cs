namespace RenStore.Delivery.Application.Features.Country.Commands.Create;

internal sealed class CreateCountryCommandHandler(
    ICountryRepository countryRepository,
    ILogger<CreateCountryCommandHandler> logger)
    : IRequestHandler<CreateCountryCommand, int>
{
    public async Task<int> Handle(
        CreateCountryCommand request,
        CancellationToken    cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. Name={Name} Code={Code}",
            nameof(CreateCountryCommand), request.Name, request.Code);
 
        var country = Domain.Entities.Country.Create(
            name:      request.Name,
            nameRu:    request.NameRu,
            code:      request.Code,
            phoneCode: request.PhoneCode,
            now:       DateTimeOffset.UtcNow);

        await countryRepository.AddAsync(country, cancellationToken);
        await countryRepository.CommitAsync(cancellationToken);

        logger.LogInformation(
            "Country created. CountryId={CountryId} Code={Code}", country.Id, country.Code);

        return country.Id;
    }
}