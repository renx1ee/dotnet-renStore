using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.DTOs.Product.FullPage;

public record ProductDetailDto
{
    public Guid DetailId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ModelFeatures { get; set; } = string.Empty;
    public string DecorativeElements { get; set; } = string.Empty;
    public string Equipment { get; set; } = string.Empty;
    public string Composition { get; set; } = string.Empty;
    public string CaringOfThings { get; set; } = string.Empty;
    public TypeOfPackaging? TypeOfPacking { get; set; }
    public int CountryOfManufactureId { get; set; }
    public Guid ProductVariantId { get; set; }
}