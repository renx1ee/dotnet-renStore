using Microsoft.EntityFrameworkCore;
using RenStore.Identity.Application.Abstractions.Projections;
using RenStore.Identity.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Identity.Persistence.Write.Projections;

internal sealed class RoleProjection(IdentityDbContext context) : IRoleProjection
{
    public async Task CommitAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);

    public async Task AddAsync(RoleReadModel role, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(role);
        await context.Roles.AddAsync(role, cancellationToken);
    }

    public async Task UpdateAsync(
        DateTimeOffset now, Guid roleId,
        string name, string normalizedName, string description,
        CancellationToken cancellationToken)
    {
        var role = await GetAsync(roleId, cancellationToken);
        role.Name           = name;
        role.NormalizedName = normalizedName;
        role.Description    = description;
        role.UpdatedAt      = now;
    }

    public async Task SetDeletedAsync(DateTimeOffset now, Guid roleId, CancellationToken cancellationToken)
    {
        var role = await GetAsync(roleId, cancellationToken);
        role.IsDeleted = true;
        role.DeletedAt = now;
        role.UpdatedAt = now;
    }

    private async Task<RoleReadModel> GetAsync(Guid roleId, CancellationToken cancellationToken)
    {
        return await context.Roles
                   .FirstOrDefaultAsync(x => x.Id == roleId, cancellationToken)
               ?? throw new NotFoundException(typeof(RoleReadModel), roleId);
    }
}