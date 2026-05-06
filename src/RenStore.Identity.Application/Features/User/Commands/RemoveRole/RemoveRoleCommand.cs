namespace RenStore.Identity.Application.Features.User.Commands.RemoveRole;

public sealed record RemoveRoleCommand(Guid UserId, Guid RoleId) : IRequest;