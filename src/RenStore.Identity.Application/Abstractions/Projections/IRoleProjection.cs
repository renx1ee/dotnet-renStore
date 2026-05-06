using RenStore.Identity.Domain.ReadModels;

namespace RenStore.Identity.Application.Abstractions.Projections;

public interface IRoleProjection
{
    Task CommitAsync(CancellationToken cancellationToken);

    Task AddAsync(RoleReadModel role, CancellationToken cancellationToken);

    Task UpdateAsync(DateTimeOffset now, Guid roleId, string name, string normalizedName, string description, CancellationToken cancellationToken);

    Task SetDeletedAsync(DateTimeOffset now, Guid roleId, CancellationToken cancellationToken);
}