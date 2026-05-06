namespace RenStore.Delivery.Application.Features.DeliveryTariff.Commands.Delete;

internal sealed class DeleteDeliveryTariffCommandHandler(
    IDeliveryTariffRepository tariffRepository,
    ILogger<DeleteDeliveryTariffCommandHandler> logger)
    : IRequestHandler<DeleteDeliveryTariffCommand>
{
    public async Task Handle(
        DeleteDeliveryTariffCommand request,
        CancellationToken           cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. TariffId={TariffId}",
            nameof(DeleteDeliveryTariffCommand), request.TariffId);

        await tariffRepository.DeleteAsync(request.TariffId, cancellationToken);
        await tariffRepository.CommitAsync(cancellationToken);

        logger.LogInformation("Tariff deleted. TariffId={TariffId}", request.TariffId);
    }
}