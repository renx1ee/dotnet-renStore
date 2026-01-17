namespace RenStore.Catalog.Domain.DTOs.Product.FullPage;

public record ProductAttributeDto
{
    public Guid AttributeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public Guid ProductVariantId { get; set; }
}