namespace RenStore.Delivery.WebApi.Requests;

public sealed record CreateAddressRequest(
    int     CountryId,
    int     CityId,
    string  Street,
    string  HouseCode,
    string  BuildingNumber,
    string  Postcode,
    string? ApartmentNumber = null,
    string? Entrance        = null,
    int?    Floor           = null);