using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Abstractions.Queries;

public interface ICountryQuery
{
    Task<CountryReadModel?> FindByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<CountryReadModel>> FindAllAsync(
        bool? isDeleted = null,
        CancellationToken cancellationToken = default);
}