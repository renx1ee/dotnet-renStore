namespace RenStore.Identity.Application.Features.Role.Commands.DeleteRole;

public sealed record DeleteRoleCommand(Guid RoleId) : IRequest;