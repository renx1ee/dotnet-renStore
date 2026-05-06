using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.Address.Commands.Create;

public sealed record CreateAddressCommand(
    Guid    UserId,
    int     CountryId,
    int     CityId,
    string  Street,
    string  HouseCode,
    string  BuildingNumber,
    string  Postcode,
    string? ApartmentNumber = null,
    string? Entrance        = null,
    int?    Floor           = null) 
    : IRequest<Guid>;