namespace RenStore.Delivery.Application.Features.City.Commands.Update;

public sealed record UpdateCityCommand(
    int    CityId,
    string Name,
    string NameRu) : IRequest;