namespace RenStore.Catalog.WebApi.Requests.Variant;

public sealed record UpdateAttributeRequest(
    string Key,
    string Value);