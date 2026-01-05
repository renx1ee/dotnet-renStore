using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.Interfaces;

namespace RenStore.Delivery.Persistence.Repositories;

public class DeliveryTariffRepository
    (ApplicationDbContext context)
    : IDeliveryTariffRepository
{
    private readonly ApplicationDbContext _context = context 
                                                     ?? throw new ArgumentNullException(nameof(context));

    public async Task<Guid> AddAsync(
        DeliveryTariff tariff,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(tariff);

        var result = await this._context.DeliveryTariffs.AddAsync(tariff, cancellationToken);

        return result.Entity.Id;
    }

    public async Task AddRangeAsync(
        IReadOnlyCollection<DeliveryTariff> tariffs,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(tariffs);

        var tariffsList = tariffs as IList<DeliveryTariff> ?? tariffs.ToList();

        if (tariffsList.Count == 0)
            return;

        await this._context.DeliveryTariffs.AddRangeAsync(tariffsList, cancellationToken);
    }

    public void Remove(DeliveryTariff tariff)
    {
        ArgumentNullException.ThrowIfNull(tariff);

        this._context.DeliveryTariffs.Remove(tariff);
    }
    
    public void Remove(IReadOnlyCollection<DeliveryTariff> tariffs)
    {
        ArgumentNullException.ThrowIfNull(tariffs);

        this._context.DeliveryTariffs.RemoveRange(tariffs);
    }
}