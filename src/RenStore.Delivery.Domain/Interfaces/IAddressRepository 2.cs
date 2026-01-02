using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Domain.Interfaces;

public interface IAddressRepository
{
    Task<Guid> AddAsync(
        Address address,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<Address> addresses,
        CancellationToken cancellationToken);

    void Remove(Address address);

    void RemoveRange(IReadOnlyCollection<Address> addressEntity);
}