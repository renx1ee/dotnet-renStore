using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.Entities;

public class ProductDetailEntity
{
    public Guid Id { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string ModelFeatures { get; private set; } = string.Empty;
    public string DecorativeElements { get; private set; } = string.Empty;
    public string Equipment { get; private set; } = string.Empty;
    public string Composition { get; private set; } = string.Empty;
    public string CaringOfThings { get; private set; } = string.Empty;
    public TypeOfPackaging? TypeOfPacking { get; private set; }
    /*public Country? CountryOfManufacture { get; set; }*/
    public int CountryOfManufactureId { get; private set; }
    public ProductVariant? ProductVariant { get; private set; }
    public Guid ProductVariantId { get; private set; }
}