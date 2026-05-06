using Microsoft.EntityFrameworkCore;
using RenStore.Identity.Application.Abstractions.Projections;
using RenStore.Identity.Domain.Enums;
using RenStore.Identity.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Identity.Persistence.Write.Projections;

internal sealed class UserProjection(IdentityDbContext context) : IUserProjection
{
    public async Task CommitAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);

    public async Task AddAsync(ApplicationUserReadModel user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        await context.Users.AddAsync(user, cancellationToken);
    }

    public async Task SetEmailAsync(DateTimeOffset now, Guid userId, string email, CancellationToken cancellationToken)
    {
        var user = await GetAsync(userId, cancellationToken);
        user.Email         = email;
        user.EmailConfirmed = false;
        user.UpdatedAt     = now;
    }

    public async Task SetEmailConfirmedAsync(DateTimeOffset now, Guid userId, CancellationToken cancellationToken)
    {
        var user = await GetAsync(userId, cancellationToken);
        user.EmailConfirmed = true;
        user.Status         = ApplicationUserStatus.IsActive;
        user.UpdatedAt      = now;
    }

    public async Task SetPasswordHashAsync(DateTimeOffset now, Guid userId, string hash, CancellationToken cancellationToken)
    {
        var user = await GetAsync(userId, cancellationToken);
        user.PasswordHash = hash;
        user.UpdatedAt    = now;
    }

    public async Task SetStatusAsync(DateTimeOffset now, Guid userId, ApplicationUserStatus status, CancellationToken cancellationToken)
    {
        var user = await GetAsync(userId, cancellationToken);
        user.Status    = status;
        user.UpdatedAt = now;
    }

    public async Task SetLockoutAsync(DateTimeOffset now, Guid userId, DateTimeOffset lockoutEnd, CancellationToken cancellationToken)
    {
        var user = await GetAsync(userId, cancellationToken);
        user.Status            = ApplicationUserStatus.Locked;
        user.LockoutEnd        = lockoutEnd;
        user.AccessFailedCount = 0;
        user.UpdatedAt         = now;
    }

    public async Task IncrementAccessFailedCountAsync(DateTimeOffset now, Guid userId, CancellationToken cancellationToken)
    {
        var user = await GetAsync(userId, cancellationToken);
        user.AccessFailedCount++;
        user.UpdatedAt = now;
    }

    public async Task ResetAccessFailedCountAsync(DateTimeOffset now, Guid userId, CancellationToken cancellationToken)
    {
        var user = await GetAsync(userId, cancellationToken);
        user.AccessFailedCount = 0;
        user.LockoutEnd        = null;
        user.UpdatedAt         = now;
    }

    public async Task SetDeletedAsync(DateTimeOffset now, Guid userId, CancellationToken cancellationToken)
    {
        var user = await GetAsync(userId, cancellationToken);
        user.Status    = ApplicationUserStatus.IsDeleted;
        user.DeletedAt = now;
        user.UpdatedAt = now;
    }

    public async Task RestoreAsync(DateTimeOffset now, Guid userId, CancellationToken cancellationToken)
    {
        var user = await GetAsync(userId, cancellationToken);
        user.Status    = ApplicationUserStatus.IsActive;
        user.DeletedAt = null;
        user.UpdatedAt = now;
    }

    private async Task<ApplicationUserReadModel> GetAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await context.Users
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
            ?? throw new NotFoundException(typeof(ApplicationUserReadModel), userId);
    }
}