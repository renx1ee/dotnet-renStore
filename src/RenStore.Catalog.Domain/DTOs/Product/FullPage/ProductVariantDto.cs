namespace RenStore.Catalog.Domain.DTOs.Product.FullPage;

public class ProductVariantDto
    
{
    public Guid VariantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Rating { get; set; }
    public long Article { get; set; }
    public int InStock { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Url { get; set; } = string.Empty;
    public Guid ProductId { get; set; }
    public int ColorId { get; set; }
}