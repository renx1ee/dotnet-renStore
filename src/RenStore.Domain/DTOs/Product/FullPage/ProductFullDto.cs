namespace RenStore.Domain.DTOs.Product.FullPage;

public class ProductFullDto
{
    public ProductDto? Product { get; set; } 
    public SellerDto? Seller { get; set; }
    public ProductClothDto? Cloth { get; set; }
    public IList<ProductDetailDto>? Details { get; set; }
    public IList<ProductAttributeDto>? Attributes { get; set; }
    public IList<ProductVariantDto>? Variants { get; set; }
    public IList<ProductClothSizeDto>? ClothSizes { get; set; }
    public IReadOnlyList<ProductPriceHistoryDto>? Prices { get; set; }
}
