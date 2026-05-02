using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Abstractions.Projections;

public interface IAddressProjection
{
    Task CommitAsync(CancellationToken cancellationToken);
    Task AddAsync(AddressReadModel address, CancellationToken cancellationToken);
    Task UpdateAsync(DateTimeOffset now, Guid addressId, string street, string houseCode, string buildingNumber, string? apartmentNumber, string? entrance, int? floor, string fullAddressEn, string fullAddressRu, CancellationToken cancellationToken);
    Task SetDeletedAsync(DateTimeOffset now, Guid addressId, CancellationToken cancellationToken);
}