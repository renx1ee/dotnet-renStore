namespace RenStore.Delivery.WebApi.Requests;

public sealed record CreateCountryRequest(
    string Name,
    string NameRu,
    string Code,
    string PhoneCode);