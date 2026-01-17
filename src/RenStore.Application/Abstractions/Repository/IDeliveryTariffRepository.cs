using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using DeliveryTariffSortBy = RenStore.Delivery.Domain.Enums.Sorting.DeliveryTariffSortBy;

namespace RenStore.Domain.Repository;

public interface IDeliveryTariffRepository
{
    Task<Guid> CreateAsync(DeliveryTariff tariff, CancellationToken cancellationToken);
    Task UpdateAsync(DeliveryTariff tariff, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<DeliveryTariff>> FindAllAsync(
        CancellationToken cancellationToken,
        DeliveryTariffSortBy sortBy = DeliveryTariffSortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false);
    Task<DeliveryTariff?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<DeliveryTariff?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}