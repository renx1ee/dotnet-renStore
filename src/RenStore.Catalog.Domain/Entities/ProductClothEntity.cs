using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Domain.Enums.Clothes;

namespace RenStore.Catalog.Domain.Entities;

public class ProductClothEntity
{
    public Guid Id { get; private set; }
    public Gender? Gender { get; private set; }
    public Season? Season { get; private set; }
    public Neckline? Neckline { get; private set; }
    public TheCut? TheCut { get; private set; }
    public Guid ProductId { get; private set; }
    public ProductEntity? Product { get; private set; }
    public IEnumerable<ProductClothSizeEntity>? ClothSizes { get; private set; }
}