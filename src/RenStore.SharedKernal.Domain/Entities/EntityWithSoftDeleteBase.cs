using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Entities;

public abstract class EntityWithSoftDeleteBase
{
    public bool IsDeleted { get; protected set; }
    
    public DateTimeOffset? UpdatedAt { get; protected set; }
    public DateTimeOffset? DeletedAt { get; protected set; }
    
    protected void EnsureNotDeleted(string? message = null)
    {
        if (IsDeleted)
            throw new DomainException(message ?? "Entity is deleted.");
    }
}