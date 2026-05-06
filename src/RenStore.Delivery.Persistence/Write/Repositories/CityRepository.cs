using Microsoft.EntityFrameworkCore;
using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.Interfaces;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Persistence.Write.Repositories;

internal sealed class CityRepository(DeliveryDbContext context) : ICityRepository
{
    public async Task CommitAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);

    public async Task AddAsync(
        City city, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(city);
        await context.Cities.AddAsync(city, cancellationToken);
    }
    
    public async Task DeleteAsync(
        int cityId, 
        CancellationToken cancellationToken)
    {
        var address = await GetAsync(cityId, cancellationToken) 
                      ?? throw new NotFoundException(typeof(CityReadModel), cityId);
        
        context.Cities.Remove(address);
    }

    public async Task<City?> GetAsync(
        int cityId, 
        CancellationToken cancellationToken)
    {
        return await context.Cities.FirstOrDefaultAsync(x =>
            x.Id == cityId, cancellationToken);
    }
}