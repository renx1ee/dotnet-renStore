using RenStore.Domain.Enums;
using RenStore.Domain.Enums.Clothes;

namespace RenStore.Domain.DTOs.Product.FullPage;

public class ProductClothDto
{
    public Guid ClothId { get; set; }
    public Gender? Gender { get; set; }
    public Season? Season { get; set; }
    public Neckline? Neckline { get; set; }
    public TheCut? TheCut { get; set; }
}