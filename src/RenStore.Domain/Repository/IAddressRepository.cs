using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;

public interface IAddressRepository
{
    Task<Guid> CreateAsync(AddressEntity address, CancellationToken cancellationToken);
    Task UpdateAsync(AddressEntity address, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<AddressEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        AddressSortBy sortBy = AddressSortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false);

    Task<AddressEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<AddressEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<AddressEntity>> FindByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        AddressSortBy sortBy = AddressSortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false);

    Task<IEnumerable<AddressEntity>> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        AddressSortBy sortBy = AddressSortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false);
}