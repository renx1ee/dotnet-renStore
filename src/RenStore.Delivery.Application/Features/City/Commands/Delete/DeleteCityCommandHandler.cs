using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Application.Features.City.Commands.Delete;

internal sealed class DeleteCityCommandHandler(
    ICityRepository cityRepository,
    ILogger<DeleteCityCommandHandler> logger)
    : IRequestHandler<DeleteCityCommand>
{
    public async Task Handle(
        DeleteCityCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. CityId={CityId}",
            nameof(DeleteCityCommand), request.CityId);

        await cityRepository.DeleteAsync(request.CityId, cancellationToken);
        await cityRepository.CommitAsync(cancellationToken);

        logger.LogInformation("City deleted. CityId={CityId}", request.CityId);
    }
}