namespace RenStore.Catalog.WebApi.Requests.Variant;

public sealed record AddAttributeToVariantRequest(
    string Key,
    string Value);