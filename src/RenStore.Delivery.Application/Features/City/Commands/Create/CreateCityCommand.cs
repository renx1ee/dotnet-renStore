using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.City.Commands.Create;

public sealed record CreateCityCommand(
    string Name,
    string NameRu,
    int    CountryId) : IRequest<int>;