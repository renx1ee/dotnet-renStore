using RenStore.Delivery.Domain.Enums;
using RenStore.Delivery.Domain.ValueObjects;
using RenStore.SharedKernal.Domain.Exceptions;
using RenStore.SharedKernal.Domain.ValueObjects;

namespace RenStore.Delivery.Domain.Entities;

/// <summary>
/// Represents a delivery tariff physical entity with life cycle and invariants.
/// </summary>
public class DeliveryTariff
{
    private readonly List<DeliveryOrder> _orders = new();
    
    public int Id { get; private set; } 
    public Price Price { get; private set; } // TODO: написат тесты
    public WeightLimitKg WeightLimitKg { get; private set; }
    public DeliveryTariffType Type { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public IReadOnlyList<DeliveryOrder> DeliveryOrders => _orders.AsReadOnly();
    
    private DeliveryTariff() { }
    
    /// <summary>
    /// Create a new Delivery Tariff insuring all invariants are satisfied.
    /// </summary>
    /// <exception cref="DomainException">if the Delivery Tariff parameters are null or empty, or any IDs are less 0.</exception>
    public static DeliveryTariff Create(
        Price price,
        DeliveryTariffType type,
        string description,
        WeightLimitKg weightLimitKg,
        DateTimeOffset now)
    {
        var tariff = new DeliveryTariff()
        {
            Price = price ?? throw new DomainException("Price cannot be null."),
            Type = type,
            WeightLimitKg = weightLimitKg ?? throw new DomainException("Weight limit must be greater then zero."),
            CreatedAt = now
        };

        if (!string.IsNullOrWhiteSpace(description))
            tariff.Description = description;

        return tariff;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="DomainException"></exception>
    public void ChangePrice(
        Price newPrice,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        Price = newPrice ?? throw new DomainException("Price cannot be null.");
        
        UpdatedAt = now;
    }
    
    public void ChangeWeightLimitKg(
        WeightLimitKg weightLimitKg,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        WeightLimitKg = weightLimitKg ?? throw new DomainException("Weight limit is requered.");
        UpdatedAt = now;
    }

    public void ChangeDescription(
        string description,
        DateTimeOffset now)
    {
        EnsureNotDeleted();

        if (Description == description) return;
        
        Description = description ?? string.Empty;
        UpdatedAt = now;
    }
    
    public void Delete(DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        IsDeleted = true;
        DeletedAt = now;
    }

    private void EnsureNotDeleted()
    {
        if(IsDeleted) 
            throw new DomainException("Delivery tariff already deleted.");
    }
}
