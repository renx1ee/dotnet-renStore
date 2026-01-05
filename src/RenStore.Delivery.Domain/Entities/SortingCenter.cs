using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Domain.Entities;

public class SortingCenter
{
    public long Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public Guid AddressId { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; } = null;
    
    private SortingCenter() { }
    
    public static SortingCenter Create(
        string code,
        Guid addressId,
        DateTimeOffset now)
    {
        if (string.IsNullOrEmpty(code))
            throw new DomainException("Code cannot be null or empty.");

        if (addressId == Guid.Empty)
            throw new DomainException("Address Id cannot be null.");
        
        return new SortingCenter()
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
            throw new DomainException("Cannot delete already deleted Sorting Center.");

        IsDeleted = true;
        DeletedAt = now;
    }
}