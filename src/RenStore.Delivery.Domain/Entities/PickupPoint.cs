using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Domain.Entities;

public class PickupPoint
{
    public long Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public bool IsDeleted { get; private set; }
    public Guid AddressId { get; private set; }
    private Address? _address { get; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; } = null;
    
    private PickupPoint() { }

    public static PickupPoint Create(
        string code,
        Guid addressId,
        DateTimeOffset now)
    {
        if (string.IsNullOrEmpty(code))
            throw new DomainException("Code cannot be null or empty.");

        if (addressId == Guid.Empty)
            throw new DomainException("Address Id cannot be null.");
        
        return new PickupPoint()
        {
            Code = code,
            AddressId = addressId,
            CreatedAt = now,
            IsDeleted = false
        };
    }
    
    public void Delete(DateTimeOffset now)
    {
        if (IsDeleted)
            throw new DomainException("Cannot delete already deleted Pickup Point.");

        IsDeleted = true;
        DeletedAt = now;
    }
}