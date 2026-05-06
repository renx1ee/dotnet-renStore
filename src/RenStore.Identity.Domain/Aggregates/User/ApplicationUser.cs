using RenStore.Identity.Domain.Aggregates.User.Events;
using RenStore.Identity.Domain.Enums;
using RenStore.Identity.Domain.ValueObjects;
using RenStore.SharedKernal.Domain.Common;
using RenStore.SharedKernal.Domain.Exceptions;

public sealed class ApplicationUser : AggregateRoot
{
    private readonly HashSet<Guid> _roleIds = new();

    public Guid Id { get; private set; }
    public UserName Name { get; private set; }
    public UserEmail Email { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public string PasswordHash { get; private set; }

    public UserPhone? Phone { get; private set; }
    public bool PhoneConfirmed { get; private set; }

    public int AccessFailedCount { get; private set; }
    public DateTimeOffset? LockoutEnd { get; private set; }

    public ApplicationUserStatus Status { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }

    public IReadOnlyCollection<Guid> RoleIds => _roleIds;

    private ApplicationUser() { }
    
    public static ApplicationUser Register(
        string firstName,
        string lastName,
        string email,
        string passwordHash,
        DateTimeOffset now)
    {
        var user = new ApplicationUser();

        var id = Guid.NewGuid();
        var name = UserName.Create(firstName, lastName);
        var userEmail = UserEmail.Create(email);

        user.Raise(new UserRegisteredEvent(
            Guid.NewGuid(),
            now,
            id,
            name,
            userEmail,
            passwordHash));

        user.Raise(new EmailVerificationRequestedEvent(
            Guid.NewGuid(),
            now,
            id,
            Guid.NewGuid()));

        return user;
    }
    
    public void ChangeEmail(string email, DateTimeOffset now)
    {
        EnsureActive();

        var newEmail = UserEmail.Create(email);

        if (Email == newEmail) return;

        Raise(new UserEmailChangedEvent(
            Guid.NewGuid(),
            now,
            Id,
            newEmail));

        Raise(new EmailVerificationRequestedEvent(
            Guid.NewGuid(),
            now,
            Id,
            Guid.NewGuid()));
    }

    public void ConfirmEmail(DateTimeOffset now)
    {
        EnsureActive();

        if (EmailConfirmed)
            throw new DomainException("Already confirmed.");

        Raise(new UserEmailConfirmedEvent(
            Guid.NewGuid(),
            now,
            Id));
    }
    
    public void ChangePassword(string hash, DateTimeOffset now)
    {
        EnsureActive();

        if (PasswordHash == hash) return;

        Raise(new UserPasswordChangedEvent(
            Guid.NewGuid(),
            now,
            Id,
            hash));
    }
    
    public void LoginSucceeded(DateTimeOffset now)
    {
        EnsureActive();

        Raise(new UserLoginSucceededEvent(
            Guid.NewGuid(),
            now,
            Id));
    }

    public void LoginFailed(DateTimeOffset now)
    {
        EnsureActive();

        Raise(new UserLoginFailedEvent(
            Guid.NewGuid(),
            now,
            Id));

        if (AccessFailedCount + 1 >= 5)
        {
            Raise(new UserLockedDueToFailuresEvent(
                Guid.NewGuid(),
                now,
                Id,
                now.AddMinutes(15)));
        }
    }
    
    public void AssignRole(Guid roleId, DateTimeOffset now)
    {
        EnsureActive();

        if (_roleIds.Contains(roleId)) return;

        Raise(new UserRoleAssignedEvent(
            Guid.NewGuid(),
            now,
            Id,
            roleId));
    }

    public void RemoveRole(Guid roleId, DateTimeOffset now)
    {
        EnsureActive();

        if (!_roleIds.Contains(roleId))
            throw new DomainException("Role not assigned.");

        Raise(new UserRoleRemovedEvent(
            Guid.NewGuid(),
            now,
            Id,
            roleId));
    }
    
    public void Delete(DateTimeOffset now)
    {
        EnsureNotDeleted();

        Raise(new UserDeletedEvent(
            Guid.NewGuid(),
            now,
            Id));
    }

    public void Restore(DateTimeOffset now)
    {
        if (Status != ApplicationUserStatus.IsDeleted)
            throw new DomainException("Not deleted.");

        Raise(new UserRestoredEvent(
            Guid.NewGuid(),
            now,
            Id));
    }
    
    protected override void Apply(IDomainEvent @event)
    {
        switch (@event)
        {
            case UserRegisteredEvent e:
                Id = e.UserId;
                Name = e.Name;
                Email = e.Email;
                PasswordHash = e.PasswordHash;
                Status = ApplicationUserStatus.UnderReview;
                CreatedAt = e.OccurredAt;
                break;

            case UserEmailChangedEvent e:
                Email = e.Email;
                EmailConfirmed = false;
                UpdatedAt = e.OccurredAt;
                break;

            case UserEmailConfirmedEvent e:
                EmailConfirmed = true;
                UpdatedAt = e.OccurredAt;
                break;

            case UserPasswordChangedEvent e:
                PasswordHash = e.PasswordHash;
                UpdatedAt = e.OccurredAt;
                break;

            case UserLoginFailedEvent e:
                AccessFailedCount++;
                UpdatedAt = e.OccurredAt;
                break;

            case UserLoginSucceededEvent e:
                AccessFailedCount = 0;
                UpdatedAt = e.OccurredAt;
                break;

            case UserLockedDueToFailuresEvent e:
                Status = ApplicationUserStatus.Locked;
                LockoutEnd = e.LockoutEnd;
                UpdatedAt = e.OccurredAt;
                break;

            case UserRoleAssignedEvent e:
                _roleIds.Add(e.RoleId);
                UpdatedAt = e.OccurredAt;
                break;

            case UserRoleRemovedEvent e:
                _roleIds.Remove(e.RoleId);
                UpdatedAt = e.OccurredAt;
                break;

            case UserDeletedEvent e:
                Status = ApplicationUserStatus.IsDeleted;
                DeletedAt = e.OccurredAt;
                UpdatedAt = e.OccurredAt;
                break;

            case UserRestoredEvent e:
                Status = ApplicationUserStatus.IsActive;
                DeletedAt = null;
                UpdatedAt = e.OccurredAt;
                break;
        }
    }

    public static ApplicationUser Rehydrate(IEnumerable<IDomainEvent> events)
    {
        var user = new ApplicationUser();

        foreach (var e in events)
        {
            user.Apply(e);
            user.Version++;
        }

        return user;
    }
    
    private void EnsureActive()
    {
        if (Status == ApplicationUserStatus.IsDeleted)
            throw new DomainException("Deleted.");

        if (Status == ApplicationUserStatus.Locked)
            throw new DomainException("Locked.");
    }

    private void EnsureNotDeleted()
    {
        if (Status == ApplicationUserStatus.IsDeleted)
            throw new DomainException("Already deleted.");
    }
}