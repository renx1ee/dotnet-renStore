using Microsoft.EntityFrameworkCore;
using RenStore.Delivery.Domain.Aggregates.Address;
using RenStore.Delivery.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Persistence.Write.Repositories;

internal sealed class AddressRepository(DeliveryDbContext context) : IAddressRepository
{
    public async Task CommitAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);

    public async Task AddAsync(Address address, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(address);
        await context.Addresses.AddAsync(address, cancellationToken);
    }
    
    public async Task DeleteAsync(
        Guid addressId, 
        CancellationToken cancellationToken)
    {
        var address = await GetAsyncById(addressId, cancellationToken)
                      ?? throw new NotFoundException(typeof(Address), addressId);
        
        context.Addresses.Remove(address);
    }

    public async Task<Address?> GetAsyncById(
        Guid addressId, 
        CancellationToken cancellationToken)
    {
        return await context.Addresses
            .FirstOrDefaultAsync(x => x.Id == addressId, cancellationToken);
    }
}