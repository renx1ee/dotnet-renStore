namespace RenStore.Domain.Entities;

public class ProductVariantEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public decimal Rating { get; set; }
    public long Article { get; set; }
    public int InStock { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Url { get; set; } = string.Empty;
    public Guid ProductId { get; set; }
    public ProductEntity? Product { get; set; }
    public int ColorId { get; set; }
    public ColorEntity? Color { get; set; }
    public ProductDetailEntity? ProductDetails { get; set; }
    public IEnumerable<ProductAttributeEntity>? ProductAttributes { get; set; } 
    public IEnumerable<ProductPriceHistoryEntity>? PriceHistories { get; set; }
    public IEnumerable<ProductImageEntity>? Images { get; set; }
    public IEnumerable<ReviewEntity>? Reviews { get; set; }
    public IEnumerable<ProductQuestionEntity>? ProductQuestions { get; set; }
}