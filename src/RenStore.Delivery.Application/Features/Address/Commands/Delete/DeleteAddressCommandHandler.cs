namespace RenStore.Delivery.Application.Features.Address.Commands.Delete;

internal sealed class DeleteAddressCommandHandler(
    IAddressRepository addressRepository,
    ILogger<DeleteAddressCommandHandler> logger)
    : IRequestHandler<DeleteAddressCommand>
{
    public async Task Handle(
        DeleteAddressCommand request,
        CancellationToken    cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. AddressId={AddressId}",
            nameof(DeleteAddressCommand), request.AddressId);

        await addressRepository.DeleteAsync(request.AddressId, cancellationToken);
        await addressRepository.CommitAsync(cancellationToken);

        logger.LogInformation("Address deleted. AddressId={AddressId}", request.AddressId);
    }
}