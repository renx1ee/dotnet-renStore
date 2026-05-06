using RenStore.Identity.Domain.Aggregates.Role.Events;
using RenStore.SharedKernal.Domain.Common;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Identity.Domain.Aggregates.Role;

public sealed class ApplicationRole : AggregateRoot
{
    public Guid            Id             { get; private set; }
    public string          Name           { get; private set; } = null!;
    public string          NormalizedName { get; private set; } = null!;
    public string          Description    { get; private set; } = string.Empty;
    public bool            IsDeleted      { get; private set; }
    public DateTimeOffset  CreatedAt      { get; private set; }
    public DateTimeOffset? UpdatedAt      { get; private set; }
    public DateTimeOffset? DeletedAt      { get; private set; }

    private ApplicationRole() { }

    public static ApplicationRole Create(
        string         name,
        string         description,
        DateTimeOffset now)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > 64)
            throw new DomainException("Role name is invalid.");

        var role = new ApplicationRole();

        role.Raise(new RoleCreatedEvent(
            EventId:        Guid.NewGuid(),
            OccurredAt:     now,
            RoleId:         Guid.NewGuid(),
            Name:           name.Trim(),
            NormalizedName: name.Trim().ToUpperInvariant(),
            Description:    description?.Trim() ?? string.Empty));

        return role;
    }

    public void Update(
        string         name,
        string         description,
        DateTimeOffset now)
    {
        EnsureNotDeleted();

        if (string.IsNullOrWhiteSpace(name) || name.Length > 64)
            throw new DomainException("Role name is invalid.");

        Raise(new RoleUpdatedEvent(
            EventId:        Guid.NewGuid(),
            OccurredAt:     now,
            RoleId:         Id,
            Name:           name.Trim(),
            NormalizedName: name.Trim().ToUpperInvariant(),
            Description:    description?.Trim() ?? string.Empty));
    }

    public void Delete(DateTimeOffset now)
    {
        EnsureNotDeleted();

        Raise(new RoleDeletedEvent(
            EventId:    Guid.NewGuid(),
            OccurredAt: now,
            RoleId:     Id));
    }

    protected override void Apply(IDomainEvent @event)
    {
        switch (@event)
        {
            case RoleCreatedEvent e:
                Id             = e.RoleId;
                Name           = e.Name;
                NormalizedName = e.NormalizedName;
                Description    = e.Description;
                IsDeleted      = false;
                CreatedAt      = e.OccurredAt;
                break;

            case RoleUpdatedEvent e:
                Name           = e.Name;
                NormalizedName = e.NormalizedName;
                Description    = e.Description;
                UpdatedAt      = e.OccurredAt;
                break;

            case RoleDeletedEvent e:
                IsDeleted = true;
                DeletedAt = e.OccurredAt;
                UpdatedAt = e.OccurredAt;
                break;
        }
    }

    public static ApplicationRole Rehydrate(IEnumerable<IDomainEvent> events)
    {
        var role = new ApplicationRole();

        foreach (var e in events)
        {
            role.Apply(e);
            role.Version++;
        }

        return role;
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted)
            throw new DomainException("Role is deleted.");
    }
}