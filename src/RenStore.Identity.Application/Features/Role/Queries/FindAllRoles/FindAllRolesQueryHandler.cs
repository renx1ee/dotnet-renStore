namespace RenStore.Identity.Application.Features.Role.Queries.FindAllRoles;

internal sealed class FindAllRolesQueryHandler(
    IApplicationRoleQuery roleQuery,
    ILogger<FindAllRolesQueryHandler> logger)
    : IRequestHandler<FindAllRolesQuery, IReadOnlyList<RoleReadModel>>
{
    public async Task<IReadOnlyList<RoleReadModel>> Handle(
        FindAllRolesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await roleQuery.FindAllAsync(request.IsDeleted, cancellationToken);
        logger.LogInformation("Fetched {Count} roles.", result.Count);
        return result;
    }
}