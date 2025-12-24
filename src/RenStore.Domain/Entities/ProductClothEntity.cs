using RenStore.Domain.Enums;
using RenStore.Domain.Enums.Clothes;

namespace RenStore.Domain.Entities;

public class ProductClothEntity
{
    public Guid Id { get; set; }
    public Gender? Gender { get; set; }
    public Season? Season { get; set; }
    public Neckline? Neckline { get; set; }
    public TheCut? TheCut { get; set; }
    public Guid ProductId { get; set; }
    public ProductEntity? Product { get; set; }
    public IEnumerable<ProductClothSizeEntity>? ClothSizes { get; set; }
}