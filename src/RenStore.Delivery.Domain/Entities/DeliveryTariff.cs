using RenStore.Delivery.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Domain.Entities;

public class DeliveryTariff
{
    private readonly List<DeliveryOrder> _orders = new();
    
    public Guid Id { get; private set; } 
    public decimal Price { get; private set; } 
    public DeliveryTariffType Type { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal WeightLimitKg { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public IReadOnlyList<DeliveryOrder> DeliveryOrders => _orders.AsReadOnly();
    
    private DeliveryTariff() { }
    
    public static DeliveryTariff Create(
        decimal price,
        DeliveryTariffType type,
        string description,
        decimal weightLimitKg,
        DateTimeOffset now)
    {
        if (price <= 0)
            throw new DomainException("Price must be greater then zero.");
        
        if (weightLimitKg <= 0)
            throw new DomainException("Weight limit must be greater then zero.");
        
        var tariff = new DeliveryTariff()
        {
            Id = Guid.NewGuid(),
            Price = price,
            Type = type,
            WeightLimitKg = weightLimitKg,
            CreatedAt = now
        };

        if (!string.IsNullOrEmpty(description))
            tariff.Description = description;

        return tariff;
    }

    public void ChangePrice(
        decimal price,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        if (price <= 0)
            throw new DomainException("Price must be greater then zero.");

        Price = price;
        UpdatedAt = now;
    }

    public void ChangeWeightLimitKg(
        decimal weightLimitKg,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        if (weightLimitKg <= 0)
            throw new DomainException("Weight limit must be greater then zero.");

        WeightLimitKg = weightLimitKg;
        UpdatedAt = now;
    }

    public void ChangeDescription(
        string description,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
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
