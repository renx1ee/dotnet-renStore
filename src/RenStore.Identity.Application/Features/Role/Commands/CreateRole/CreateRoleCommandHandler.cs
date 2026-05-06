using RenStore.Identity.Domain.Aggregates.Role;

namespace RenStore.Identity.Application.Features.Role.Commands.CreateRole;

internal sealed class CreateRoleCommandHandler(
    IRoleRepository roleRepository,
    ILogger<CreateRoleCommandHandler> logger)
    : IRequestHandler<CreateRoleCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateRoleCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. Name={Name}",
            nameof(CreateRoleCommand), request.Name);

        var role = ApplicationRole.Create(
            name:        request.Name,
            description: request.Description,
            now:         DateTimeOffset.UtcNow);

        await roleRepository.SaveAsync(role, cancellationToken);

        logger.LogInformation("Role created. RoleId={RoleId}", role.Id);

        return role.Id;
    }
}