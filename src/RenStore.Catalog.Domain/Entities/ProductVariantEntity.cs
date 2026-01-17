namespace RenStore.Catalog.Domain.Entities;

public class ProductVariantEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string NormalizedName { get; private set; } = string.Empty;
    public decimal Rating { get; private set; }
    public long Article { get; private set; }
    public int InStock { get; private set; }
    public bool IsAvailable { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public string Url { get; private set; } = string.Empty;
    public Guid ProductId { get; private set; }
    public ProductEntity? Product { get; private set; }
    public int ColorId { get; private set; }
    public Color? Color { get; private set; }
    public Guid ProductDetailId { get; private set; }
    public ProductDetailEntity? ProductDetails { get; private set; }
    public Guid ComplainId { get; private set; }
    public IEnumerable<ProductAttributeEntity>? ProductAttributes { get; private set; } 
    public IEnumerable<ProductPriceHistoryEntity>? PriceHistories { get; private set; }
    public IEnumerable<ProductImageEntity>? Images { get; private set; }
    /*public IEnumerable<ReviewEntity>? Reviews { get; set; }
    public IEnumerable<ProductQuestionEntity>? ProductQuestions { get; set; }
    public IEnumerable<ProductVariantComplainEntity>? Complains { get; set; }*/
}