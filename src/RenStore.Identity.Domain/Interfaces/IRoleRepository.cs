using RenStore.Identity.Domain.Aggregates.Role;

namespace RenStore.Identity.Domain.Interfaces;

public interface IRoleRepository
{
    Task<ApplicationRole?> GetAsync(
        Guid              roleId,
        CancellationToken cancellationToken);

    Task SaveAsync(
        Aggregates.Role.ApplicationRole applicationRole,
        CancellationToken    cancellationToken);
}