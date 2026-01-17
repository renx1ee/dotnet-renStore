namespace RenStore.Domain.Entities;

public class ShoppingCartItemEntity
{
    public Guid Id { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal Price { get; set; }
    public Guid CartId { get; set; }
    public ShoppingCartEntity? Cart { get; set; }
    public Guid ProductId { get; set; }
    /*public ProductEntity? Product { get; set; }*/
}