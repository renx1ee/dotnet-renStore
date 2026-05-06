namespace RenStore.Identity.Application.Features.Role.Commands.CreateRole;

public sealed record CreateRoleCommand(
    string Name,
    string Description) : IRequest<Guid>;