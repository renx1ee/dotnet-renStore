using RenStore.Identity.Domain.Aggregates.User.Rules;
using RenStore.Identity.Domain.Enums;
using RenStore.Identity.Domain.ValueObjects;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Identity.Domain.Aggregates.User;

/// <summary>
/// Represents an application user physical entity with lifecycle and invariants.
/// </summary>
public class User 
{
    public Guid Id { get; private set; }
    public UserName Name { get; private set; }
    public UserEmail Email { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public string PasswordHash { get; private set; } // TODO: make rules
    public DateTimeOffset? LockoutEnd { get; private set; }
    public int AccessFailedCount { get; private set; }
    public ApplicationUserStatus Status { get; private set; } 
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    
    private User() { }

    public static User Create(
        string firstName,
        string lastName,
        string email,
        string passwordHash,
        DateTimeOffset now)
    {
        UserRules.ValidateCreatingUser(
            firstName: firstName,
            lastName: lastName,
            email: email,
            passwordHash: passwordHash);
        
        var userId = Guid.NewGuid();
        
        var user = new User
        {
            Id = userId,
            Status = ApplicationUserStatus.UnderReview,
            Name = UserName.Create(
                firstname: firstName, 
                lastName: lastName),
            Email = UserEmail.Create(email),
            PasswordHash = passwordHash,
            CreatedAt = now
        };

        return user;
    }

    public void ConfirmEmail(DateTimeOffset now)
    {
        EnsureNotDeleted();
        EnsureNotLocked();
        
        if (EmailConfirmed)
            throw new DomainException("Email already confirmed.");
        
        EmailConfirmed = true;
        UpdatedAt = now;
    }

    public void ChangeUserName(
        DateTimeOffset now,
        string firstName,
        string lastName)
    {
        EnsureNotDeleted();
        EnsureNotLocked();
        
        UserRules.ValidateUserName(firstName, lastName);
        
        var newName = UserName.Create(firstName, lastName);

        if (Name.Equals(newName)) return;

        Name = newName;
        UpdatedAt = now;
    }
    
    public void ChangeEmail(
        DateTimeOffset now,
        string email)
    {
        EnsureNotDeleted();
        EnsureNotLocked();
        
        UserRules.ValidateEmail(email);
        
        Email = UserEmail.Create(email);
        UpdatedAt = now;
    }
    
    public void ChangePasswordHash(
        DateTimeOffset now,
        string passwordHash)
    {
        EnsureNotDeleted();
        EnsureNotLocked();
        
        UserRules.ValidatePasswordHash(passwordHash);

        PasswordHash = passwordHash;
        UpdatedAt = now;
    }
    
    public void Lock(
        DateTimeOffset now,
        DateTimeOffset lockedEnd)
    {
        EnsureNotDeleted();

        if (Status == ApplicationUserStatus.Locked)
            return;

        Status = ApplicationUserStatus.Locked;
        
        LockoutEnd = lockedEnd;
        UpdatedAt = now;
    }
    
    public void Activate(DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        if (Status == ApplicationUserStatus.IsActive)
            return;

        Status = ApplicationUserStatus.IsActive;
        
        LockoutEnd = null;
        UpdatedAt = now;
    }
    
    public void TryToAccess(DateTimeOffset now)
    {
        EnsureNotDeleted();
        EnsureNotLocked();

        AccessFailedCount++;
        UpdatedAt = now;
    }

    public void Delete(DateTimeOffset now)
    {
        EnsureNotDeleted();

        Status = ApplicationUserStatus.IsDeleted;
        UpdatedAt = now;
        DeletedAt = now;
    }
    
    public void Restore(DateTimeOffset now)
    {
        if(Status != ApplicationUserStatus.IsDeleted)
            throw new DomainException("The user is not deleted.");
        
        Status = ApplicationUserStatus.IsActive;
        UpdatedAt = now;
        DeletedAt = null;
    }
    
    /// <summary>
    /// Ensures the user is not deleted before performing operations.
    /// </summary>
    /// <exception cref="DomainException">Thrown when entity is deleted.</exception>
    private void EnsureNotDeleted()
    {
        if(Status == ApplicationUserStatus.IsDeleted)
            throw new DomainException("The user has already been deleted.");
    }
    
    /// <summary>
    /// Ensures the user is not locked before performing operations.
    /// </summary>
    /// <exception cref="DomainException">Thrown when entity is locked.</exception>
    private void EnsureNotLocked()
    {
        if(Status == ApplicationUserStatus.Locked)
            throw new DomainException("The user is locked.");
    }
}