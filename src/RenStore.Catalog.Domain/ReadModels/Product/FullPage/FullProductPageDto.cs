namespace RenStore.Catalog.Domain.ReadModels.Product.FullPage;

public sealed class FullProductPageDto
{
    public ProductDto Product { get; init; }
    public ProductVariantDto Variant { get; init; }
    public VariantDetailsDto Details { get; init; }
    public IList<ProductVariantImageDto> Images { get; init; }
    public IList<SizeDto> SizeWithPrices { get; init; }
    public IList<VariantAttributeDto> Attributes { get; init; }
    public IList<ProductVariantLinkDto> OtherVariantsLinks { get; init; }
};