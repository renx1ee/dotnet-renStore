namespace RenStore.Identity.Application.Features.Role.Queries.FindRoleById;

internal sealed class FindRoleByIdQueryHandler(
    IApplicationRoleQuery roleQuery,
    ILogger<FindRoleByIdQueryHandler> logger)
    : IRequestHandler<FindRoleByIdQuery, RoleReadModel?>
{
    public async Task<RoleReadModel?> Handle(
        FindRoleByIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handling {Query}. RoleId={RoleId}",
            nameof(FindRoleByIdQuery), request.RoleId);

        return await roleQuery.FindByIdAsync(request.RoleId, cancellationToken);
    }
}