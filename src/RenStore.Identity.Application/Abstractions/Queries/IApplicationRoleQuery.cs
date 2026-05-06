using RenStore.Identity.Domain.ReadModels;

namespace RenStore.Identity.Application.Abstractions.Queries;

public interface IApplicationRoleQuery
{
    Task<RoleReadModel?> FindByIdAsync(
        Guid              roleId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<RoleReadModel>> FindAllAsync(
        bool?             isDeleted = false,
        CancellationToken cancellationToken = default);
}