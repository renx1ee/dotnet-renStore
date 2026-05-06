namespace RenStore.Delivery.Application.Features.Address.Commands.Edit;

public sealed record EditAddressCommand(
    Guid    AddressId,
    string  Street,
    string  HouseCode,
    string  BuildingNumber,
    string  Postcode,
    string? ApartmentNumber = null,
    string? Entrance        = null,
    int?    Floor           = null) 
    : IRequest;