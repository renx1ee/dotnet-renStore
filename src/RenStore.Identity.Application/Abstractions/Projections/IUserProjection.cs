using RenStore.Identity.Domain.ReadModels;

namespace RenStore.Identity.Application.Abstractions.Projections;

public interface IUserProjection
{
    Task CommitAsync(CancellationToken cancellationToken);

    Task AddAsync(ApplicationUserReadModel user, CancellationToken cancellationToken);

    Task SetEmailAsync(DateTimeOffset now, Guid userId, string email, CancellationToken cancellationToken);

    Task SetEmailConfirmedAsync(DateTimeOffset now, Guid userId, CancellationToken cancellationToken);

    Task SetPasswordHashAsync(DateTimeOffset now, Guid userId, string hash, CancellationToken cancellationToken);

    Task SetStatusAsync(DateTimeOffset now, Guid userId, Domain.Enums.ApplicationUserStatus status, CancellationToken cancellationToken);

    Task SetLockoutAsync(DateTimeOffset now, Guid userId, DateTimeOffset lockoutEnd, CancellationToken cancellationToken);

    Task IncrementAccessFailedCountAsync(DateTimeOffset now, Guid userId, CancellationToken cancellationToken);

    Task ResetAccessFailedCountAsync(DateTimeOffset now, Guid userId, CancellationToken cancellationToken);

    Task SetDeletedAsync(DateTimeOffset now, Guid userId, CancellationToken cancellationToken);

    Task RestoreAsync(DateTimeOffset now, Guid userId, CancellationToken cancellationToken);
}