namespace RenStore.Identity.Application.Features.User.Commands.RestoreUser;

public sealed record RestoreUserCommand(Guid UserId) : IRequest;