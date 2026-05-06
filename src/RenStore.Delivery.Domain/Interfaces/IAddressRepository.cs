using RenStore.Delivery.Domain.Aggregates.Address;

namespace RenStore.Delivery.Domain.Interfaces;

public interface IAddressRepository
{
    Task CommitAsync(CancellationToken cancellationToken);
    Task AddAsync(Address address, CancellationToken cancellationToken);

    Task DeleteAsync(Guid addressId, CancellationToken cancellationToken);

    Task<Address?> GetAsyncById(Guid addressId, CancellationToken cancellationToken);
}