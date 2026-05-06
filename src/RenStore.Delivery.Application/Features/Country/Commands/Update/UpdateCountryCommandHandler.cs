using System.Reflection;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Application.Features.Country.Commands.Update;

internal sealed class UpdateCountryCommandHandler(
    ICountryRepository countryRepository,
    ILogger<UpdateCountryCommandHandler> logger)
    : IRequestHandler<UpdateCountryCommand>
{
    public async Task Handle(
        UpdateCountryCommand request,
        CancellationToken    cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. CountryId={CountryId}",
            nameof(UpdateCountryCommand), request.CountryId);

        var country = await countryRepository.GetAsync(request.CountryId, cancellationToken);

        if (country is null)
            throw new NotFoundException(name: typeof(Domain.Entities.Country), request.CountryId);

        country.Update(
            now:               DateTimeOffset.UtcNow,
            name:              request.Name,
            nameRu:            request.NameRu,
            code:              request.Code,
            phoneCode:         request.PhoneCode);

        await countryRepository.CommitAsync(cancellationToken);

        logger.LogInformation("Country updated. CountryId={CountryId}", request.CountryId);
    }
}