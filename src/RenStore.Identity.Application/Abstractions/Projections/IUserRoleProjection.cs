namespace RenStore.Identity.Application.Abstractions.Projections;

public interface IUserRoleProjection
{
    Task CommitAsync(CancellationToken cancellationToken);

    Task AddAsync(Guid userId, Guid roleId, string roleName, CancellationToken cancellationToken);

    Task RemoveAsync(Guid userId, Guid roleId, CancellationToken cancellationToken);
}