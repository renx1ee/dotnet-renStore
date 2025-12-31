using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Shoes;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;

public interface IDeliveryTariffRepository
{
    Task<Guid> CreateAsync(DeliveryTariffEntity tariff, CancellationToken cancellationToken);
    Task UpdateAsync(DeliveryTariffEntity tariff, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<DeliveryTariffEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        DeliveryTariffSortBy sortBy = DeliveryTariffSortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false);
    Task<DeliveryTariffEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<DeliveryTariffEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}