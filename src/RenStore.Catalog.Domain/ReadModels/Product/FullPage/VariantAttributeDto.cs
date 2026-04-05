namespace RenStore.Catalog.Domain.ReadModels.Product.FullPage;

public sealed record VariantAttributeDto(
    Guid AttributeId,
    string Key,
    string Value);