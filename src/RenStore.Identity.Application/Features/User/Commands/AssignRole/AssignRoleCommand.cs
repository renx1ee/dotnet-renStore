namespace RenStore.Identity.Application.Features.User.Commands.AssignRole;

public sealed record AssignRoleCommand(Guid UserId, Guid RoleId) : IRequest;