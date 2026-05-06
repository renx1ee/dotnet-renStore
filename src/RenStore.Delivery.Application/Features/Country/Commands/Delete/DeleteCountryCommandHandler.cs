namespace RenStore.Delivery.Application.Features.Country.Commands.Delete;

internal sealed class DeleteCountryCommandHandler(
    ICountryRepository countryRepository,
    ILogger<DeleteCountryCommandHandler> logger)
    : IRequestHandler<DeleteCountryCommand>
{
    public async Task Handle(
        DeleteCountryCommand request,
        CancellationToken    cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. CountryId={CountryId}",
            nameof(DeleteCountryCommand), request.CountryId);
        
        await countryRepository.DeleteAsync(request.CountryId, cancellationToken);
        await countryRepository.CommitAsync(cancellationToken);

        logger.LogInformation("Country deleted. CountryId={CountryId}", request.CountryId);
    }
}