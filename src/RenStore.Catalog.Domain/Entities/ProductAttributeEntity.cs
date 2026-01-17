namespace RenStore.Catalog.Domain.Entities;

public class ProductAttributeEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Value { get; private set; } = string.Empty;
    public Guid ProductVariantId { get; private set; }
    public ProductVariantEntity? ProductVariant { get; private set; }
}