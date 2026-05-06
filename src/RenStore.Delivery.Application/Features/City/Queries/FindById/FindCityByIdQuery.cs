using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.City.Queries.FindById;

public sealed record FindCityByIdQuery(int CityId) : IRequest<CityReadModel?>;