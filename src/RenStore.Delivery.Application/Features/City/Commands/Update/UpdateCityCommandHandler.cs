using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Application.Features.City.Commands.Update;

internal sealed class UpdateCityCommandHandler(
    ICityRepository cityRepository,
    ILogger<UpdateCityCommandHandler> logger)
    : IRequestHandler<UpdateCityCommand>
{
    public async Task Handle(
        UpdateCityCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. CityId={CityId}",
            nameof(UpdateCityCommand), request.CityId);

        var city = await cityRepository.GetAsync(request.CityId, cancellationToken);

        if (city is null)
            throw new NotFoundException(name: typeof(Domain.Entities.City), request.CityId);
        
        city.Update(
            now:    DateTimeOffset.UtcNow,
            name:   request.Name,
            nameRu: request.NameRu);
        
        await cityRepository.CommitAsync(cancellationToken);

        logger.LogInformation("City updated. CityId={CityId}", request.CityId);
    }
}