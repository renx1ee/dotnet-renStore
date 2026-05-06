namespace RenStore.Delivery.Application.Features.City.Commands.Delete;

public sealed record DeleteCityCommand(int CityId) : IRequest;