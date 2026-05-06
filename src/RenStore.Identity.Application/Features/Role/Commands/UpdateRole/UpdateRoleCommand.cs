namespace RenStore.Identity.Application.Features.Role.Commands.UpdateRole;

public sealed record UpdateRoleCommand(
    Guid   RoleId,
    string Name,
    string Description) : IRequest;