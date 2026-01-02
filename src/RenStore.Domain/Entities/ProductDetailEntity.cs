using RenStore.Delivery.Domain.Entities;
using RenStore.Domain.Enums;

namespace RenStore.Domain.Entities;

public class ProductDetailEntity
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ModelFeatures { get; set; } = string.Empty;
    public string DecorativeElements { get; set; } = string.Empty;
    public string Equipment { get; set; } = string.Empty;
    public string Composition { get; set; } = string.Empty;
    public string CaringOfThings { get; set; } = string.Empty;
    public TypeOfPackaging? TypeOfPacking { get; set; }
    public Country? CountryOfManufacture { get; set; }
    public int CountryOfManufactureId { get; set; }
    public ProductVariantEntity? ProductVariant { get; set; }
    public Guid ProductVariantId { get; set; }
}