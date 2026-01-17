namespace RenStore.Catalog.Domain.Entities;

public class ProductEntity
{
    public Guid Id { get; private set; }
    public bool IsBlocked { get; private set; } = false;
    public decimal OverallRating { get; private set; }
    public long SellerId { get; private set; }
    /*public SellerEntity? Seller { get; set; }*/
    public int CategoryId { get; private set; }
    public Category? Category { get; private set; }
    public IEnumerable<ProductVariantEntity>? ProductVariants { get; private set; } 
    public ProductClothEntity? ProductCloth { get; private set; }/*
    public IEnumerable<ShoppingCartItemEntity>? CartItems { get; set; }
    public IEnumerable<OrderItemEntity>? OrderItems { get; set; }*/
}