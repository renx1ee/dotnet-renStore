using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Abstractions.Queries;

public interface IDeliveryTariffQuery
{
    Task<DeliveryTariffReadModel?> FindByIdAsync(
        int               tariffId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<DeliveryTariffReadModel>> FindAllAsync(
        bool?             isDeleted,
        CancellationToken cancellationToken);
}