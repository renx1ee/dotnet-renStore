using RenStore.Application.Features.Address.Queries;
using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Application.Interfaces;

public interface IAddressQuery
{
    Task<IReadOnlyList<AddressReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        AddressSortBy sortBy = AddressSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false);

    Task<IReadOnlyList<AddressReadModel>> SearchAsync(
        AddressSearchCriteria criteria,
        CancellationToken cancellationToken,
        bool? includeDeleted = null,
        AddressSortBy sortBy = AddressSortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false);

    Task<AddressReadModel?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<AddressReadModel> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<AddressReadModel>> FindByUserIdAsync(
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