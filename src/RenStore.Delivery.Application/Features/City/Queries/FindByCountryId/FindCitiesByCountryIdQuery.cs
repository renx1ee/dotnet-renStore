using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.City.Queries.FindByCountryId;

public sealed record FindCitiesByCountryIdQuery(
    int   CountryId,
    bool? IsDeleted = false) 
    : IRequest<IReadOnlyList<CityReadModel>>;