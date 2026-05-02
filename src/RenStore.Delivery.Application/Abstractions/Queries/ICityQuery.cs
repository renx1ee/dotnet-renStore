using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Abstractions.Queries;

public interface ICityQuery
{
    Task<CityReadModel?> FindByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<CityReadModel>> FindByCountryIdAsync(
        int countryId,
        bool? isDeleted = null,
        CancellationToken cancellationToken = default);
}