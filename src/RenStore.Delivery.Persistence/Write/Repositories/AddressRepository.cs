using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.Interfaces;

namespace RenStore.Delivery.Persistence.Write.Repositories;

internal sealed class AddressRepository
    (DeliveryDbContext context) 
    : RenStore.Delivery.Domain.Interfaces.IAddressRepository
{
    private readonly DeliveryDbContext _context = context 
                                                  ?? throw new ArgumentNullException(nameof(context));
    /// <summary>
    /// <inheritdoc cref="IAddressRepository.AddAsync"/>
    /// </summary>
    public async Task<Guid> AddAsync(
        Address address, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(address);
        
        await this._context.Addresses.AddAsync(address, cancellationToken);
        
        return address.Id;
    }

    public async Task AddRangeAsync(
        IReadOnlyCollection<Address> addresses, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(addresses);
        
        var addressesList = addresses as IList<Address> ?? addresses.ToList();

        if (addressesList.Count == 0) return;
        
        await this._context.Addresses.AddRangeAsync(addressesList, cancellationToken);
    }

    public void Remove(Address address)
    {
        ArgumentNullException.ThrowIfNull(address);
        
        this._context.Addresses.Remove(address);
    }
    
    public void RemoveRange(IReadOnlyCollection<Address> addressEntity)
    {
        ArgumentNullException.ThrowIfNull(addressEntity);
        
        this._context.Addresses.RemoveRange(addressEntity);
    }
}