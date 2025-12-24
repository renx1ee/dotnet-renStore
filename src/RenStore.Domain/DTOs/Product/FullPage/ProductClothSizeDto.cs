using RenStore.Domain.Enums.Clothes;

namespace RenStore.Domain.DTOs.Product.FullPage;

public class ProductClothSizeDto
{
    public Guid ClothSizeId { get; set; }
    public ClothesSizes? ClothSize { get; set; }
    public int Amount { get; set; }
    public Guid ProductClothId { get; set; }
}