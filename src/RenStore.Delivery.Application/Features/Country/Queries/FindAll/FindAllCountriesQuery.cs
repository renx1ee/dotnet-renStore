using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.Country.Queries.FindAll;

public sealed record FindAllCountriesQuery(bool? IsDeleted = false)
    : IRequest<IReadOnlyList<CountryReadModel>>;