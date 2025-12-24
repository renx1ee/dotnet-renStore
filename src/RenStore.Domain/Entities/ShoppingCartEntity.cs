namespace RenStore.Domain.Entities;

public class ShoppingCartEntity
{
    public Guid Id { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
    public DateTime? UpdatedAt { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }
    public IEnumerable<ShoppingCartItemEntity>? Items { get; set; }
}

// TODO: сделать вычисление и обновление всей стоимости через триггеры в бд