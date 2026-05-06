using Microsoft.EntityFrameworkCore;
using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Persistence.Write.Repositories;

internal sealed class DeliveryTariffRepository(DeliveryDbContext context) : IDeliveryTariffRepository
{
    public async Task CommitAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);

    public async Task<int> AddAsync(DeliveryTariff tariff, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(tariff);
        
        await context.DeliveryTariffs.AddAsync(tariff, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return tariff.Id;
    }
    
    public async Task DeleteAsync(
        int tariffId, 
        CancellationToken cancellationToken)
    {
        var address = await GetAsync(tariffId, cancellationToken)
                      ?? throw new NotFoundException(typeof(DeliveryTariff), tariffId);
        
        context.DeliveryTariffs.Remove(address);
    }

    public async Task<DeliveryTariff?> GetAsync(
        int tariffId, 
        CancellationToken cancellationToken)
    {
        return await context.DeliveryTariffs
            .FirstOrDefaultAsync(x =>
                    x.Id == tariffId,
                cancellationToken);
    }
}