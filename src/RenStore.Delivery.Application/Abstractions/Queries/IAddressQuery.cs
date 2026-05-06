using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Abstractions.Queries;

public interface IAddressQuery
{
    Task<AddressReadModel?> FindByIdAsync(
        Guid              addressId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<AddressReadModel>> FindByUserIdAsync(
        Guid              userId,
        bool?             isDeleted,
        CancellationToken cancellationToken);
}