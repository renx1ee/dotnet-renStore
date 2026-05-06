using Microsoft.EntityFrameworkCore;
using RenStore.Identity.Application.Abstractions.Projections;
using RenStore.Identity.Domain.ReadModels;

namespace RenStore.Identity.Persistence.Write.Projections;

internal sealed class UserRoleProjection(IdentityDbContext context) : IUserRoleProjection
{
    public async Task CommitAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);

    public async Task AddAsync(Guid userId, Guid roleId, string roleName, CancellationToken cancellationToken)
    {
        await context.UserRoles.AddAsync(new UserRoleReadModel
        {
            UserId   = userId,
            RoleId   = roleId,
            RoleName = roleName
        }, cancellationToken);
    }

    public async Task RemoveAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
    {
        var record = await context.UserRoles
            .FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId, cancellationToken);

        if (record is not null)
            context.UserRoles.Remove(record);
    }
}