namespace RenStore.Catalog.Domain.Entities;

public class Product
{
    private Category? _category { get; set; }
    
    public Guid Id { get; private set; }
    public bool IsBlocked { get; private set; } 
    public decimal OverallRating { get; private set; }
    public long SellerId { get; private set; }
    public int CategoryId { get; private set; }
    public IEnumerable<ProductVariant>? ProductVariants { get; private set; } 
    public ProductClothEntity? ProductCloth { get; private set; }
    
    private Product() { }

    public static Product Create()
    {
        var product = new Product()
        {
            
        };

        return product;
    }
}