namespace RenStore.Delivery.WebApi.Requests;

public sealed record EditAddressRequest(
    string  Street,
    string  HouseCode,
    string  BuildingNumber,
    string  Postcode,
    string? ApartmentNumber = null,
    string? Entrance        = null,
    int?    Floor           = null);