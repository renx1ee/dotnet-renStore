using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Domain.Enums.Clothes;

namespace RenStore.Catalog.Domain.DTOs.Product.FullPage;

public class ProductClothDto
{
    public Guid ClothId { get; set; }
    public Gender? Gender { get; set; }
    public Season? Season { get; set; }
    public Neckline? Neckline { get; set; }
    public TheCut? TheCut { get; set; }
}