using RenStore.Identity.Domain.Enums;
using RenStore.Identity.Domain.ReadModels;

namespace RenStore.Identity.Application.Abstractions.Queries;

public interface IApplicationUserQuery
{
    Task<ApplicationUserReadModel?> FindByIdAsync(
        Guid              userId,
        CancellationToken cancellationToken);

    Task<ApplicationUserReadModel?> FindByEmailAsync(
        string            email,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<ApplicationUserReadModel>> FindAllAsync(
        uint                   page = 1,
        uint                   pageSize = 25,
        bool                   descending = true,
        ApplicationUserStatus? status = null,
        CancellationToken      cancellationToken = default);
}