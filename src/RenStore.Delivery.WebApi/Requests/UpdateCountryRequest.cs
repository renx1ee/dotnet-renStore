namespace RenStore.Delivery.WebApi.Requests;

public sealed record UpdateCountryRequest(
    string Name,
    string NameRu,
    string Code,
    string PhoneCode);