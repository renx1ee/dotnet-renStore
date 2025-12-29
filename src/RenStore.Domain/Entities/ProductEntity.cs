namespace RenStore.Domain.Entities;

public class ProductEntity
{
    public Guid Id { get; set; }
    public bool IsBlocked { get; set; } = false;
    public decimal OverallRating { get; set; }
    public long SellerId { get; set; }
    public SellerEntity? Seller { get; set; }
    public int CategoryId { get; set; }
    public CategoryEntity? Category { get; set; }
    public IEnumerable<ProductVariantEntity>? ProductVariants { get; set; } 
    public ProductClothEntity? ProductCloth { get; set; }
    public IEnumerable<ShoppingCartItemEntity>? CartItems { get; set; }
    public IEnumerable<OrderItemEntity>? OrderItems { get; set; }
}