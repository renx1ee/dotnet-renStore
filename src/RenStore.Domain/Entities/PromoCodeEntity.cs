using RenStore.Domain.Enums;

namespace RenStore.Domain.Entities;

public class PromoCodeEntity
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PromoCodeType Type { get; set; }
    public PromoCodeTarget Target { get; set; }
    public string? TargetUserId { get; set; }
    public Guid? TargetSellerId { get; set; }
    public Guid? TargetCategoryId { get; set; }
    public Guid? TargetProductId { get; set; }
    public decimal? DiscountValue { get; set; }
    public decimal? MinimumOrderAmount { get; set; }
    public decimal? MaximumOrderValue { get; set; }
    public DateTime? ValidFrom { get; set; } 
    public DateTime? ValidTo { get; set; } 
    public int MaxUses { get; set; } = 1;
    public int UsedCount { get; set; } = 1;
    public bool IsActive { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool ApplyToShipping { get; set; } = false;
    public bool ApplyToCommission { get; set; } = false;
    public bool IsFirstOrderOnly { get; set; } = false;
    public bool IsNewCustomerOnly { get; set; } = false;
    public IList<PromoCodeUserLimit>? PromoCodeUserLimits { get; set; }
    public IList<OrderEntity>? Orders { get; set; }
}