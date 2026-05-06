using RenStore.Identity.Domain.Aggregates.Role;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Identity.Application.Features.Role.Commands.DeleteRole;

internal sealed class DeleteRoleCommandHandler(
    IRoleRepository roleRepository,
    ILogger<DeleteRoleCommandHandler> logger)
    : IRequestHandler<DeleteRoleCommand>
{
    public async Task Handle(
        DeleteRoleCommand request,
        CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetAsync(request.RoleId, cancellationToken)
                   ?? throw new NotFoundException(typeof(ApplicationRole), request.RoleId);

        role.Delete(DateTimeOffset.UtcNow);
        await roleRepository.SaveAsync(role, cancellationToken);

        logger.LogInformation("Role deleted. RoleId={RoleId}", request.RoleId);
    }
}