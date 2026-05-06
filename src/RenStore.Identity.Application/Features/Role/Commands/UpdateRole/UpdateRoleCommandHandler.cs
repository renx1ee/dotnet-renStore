using RenStore.Identity.Domain.Aggregates.Role;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Identity.Application.Features.Role.Commands.UpdateRole;

internal sealed class UpdateRoleCommandHandler(
    IRoleRepository roleRepository,
    ILogger<UpdateRoleCommandHandler> logger)
    : IRequestHandler<UpdateRoleCommand>
{
    public async Task Handle(
        UpdateRoleCommand request,
        CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetAsync(request.RoleId, cancellationToken)
                   ?? throw new NotFoundException(typeof(ApplicationRole), request.RoleId);

        role.Update(request.Name, request.Description, DateTimeOffset.UtcNow);
        await roleRepository.SaveAsync(role, cancellationToken);

        logger.LogInformation("Role updated. RoleId={RoleId}", request.RoleId);
    }
}