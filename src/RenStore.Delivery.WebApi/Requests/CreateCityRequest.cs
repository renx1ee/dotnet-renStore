namespace RenStore.Delivery.WebApi.Requests;

public sealed record CreateCityRequest(string Name, string NameRu, int CountryId);