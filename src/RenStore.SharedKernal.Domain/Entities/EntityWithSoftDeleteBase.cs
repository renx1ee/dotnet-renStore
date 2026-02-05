using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.SharedKernal.Domain.Entities;

public abstract class EntityWithSoftDeleteBase
{
    public bool IsDeleted { get; protected set; }
    
    
}