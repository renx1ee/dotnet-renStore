using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.Country.Queries.FindById;

public sealed record FindCountryByIdQuery(int CountryId) : IRequest<CountryReadModel?>;