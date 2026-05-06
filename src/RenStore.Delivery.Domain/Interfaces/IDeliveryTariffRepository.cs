using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Domain.Interfaces;

public interface IDeliveryTariffRepository
{
    Task CommitAsync(CancellationToken cancellationToken);
    
    Task<int> AddAsync(DeliveryTariff tariff, CancellationToken cancellationToken);

    Task DeleteAsync(int tariffId, CancellationToken cancellationToken);

    Task<DeliveryTariff?> GetAsync(int tariffId, CancellationToken cancellationToken);
}