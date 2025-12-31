using RenStore.Application.Features.Address.Queries;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Application.Abstractions.Repository;

public interface IAddressRepository
{
    Task<Guid> AddAsync(
        AddressEntity address,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<AddressEntity> addresses,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        AddressEntity address,
        CancellationToken cancellationToken);

    Task UpdateDetachedUnsafeAsync(
        AddressEntity address,
        CancellationToken cancellationToken);

    Task UpdateRangeAsync(
        IReadOnlyCollection<AddressEntity> addresses,
        CancellationToken cancellationToken);

    Task UpdateDetachedRangeUnsafeAsync(
        IReadOnlyCollection<AddressEntity> addresses,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task DeleteHardAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task DeleteRangeAsync(
        IReadOnlyCollection<AddressEntity> addresses,
        CancellationToken cancellationToken);

    Task DeleteRangeAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken);

    Task DeleteHardRangeAsync(
        IReadOnlyCollection<AddressEntity> addresses,
        CancellationToken cancellationToken);

    Task DeleteHardRangeAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken);

    Task<int> CommitAsync(
        CancellationToken cancellationToken);

    Task<IReadOnlyList<AddressEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        AddressSortBy sortBy = AddressSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false);

    Task<IReadOnlyList<AddressEntity>> SearchAsync(
        AddressSearchCriteria criteria,
        CancellationToken cancellationToken,
        bool? includeDeleted = null,
        AddressSortBy sortBy = AddressSortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false);

    Task<AddressEntity?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<AddressEntity> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<AddressEntity>> FindByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        AddressSortBy sortBy = AddressSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false);

    Task<bool> IsExists(
        Guid id,
        CancellationToken cancellationToken);
}