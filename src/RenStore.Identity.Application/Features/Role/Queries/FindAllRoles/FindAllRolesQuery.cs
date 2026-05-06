namespace RenStore.Identity.Application.Features.Role.Queries.FindAllRoles;

public sealed record FindAllRolesQuery(bool? IsDeleted = false)
    : IRequest<IReadOnlyList<RoleReadModel>>;