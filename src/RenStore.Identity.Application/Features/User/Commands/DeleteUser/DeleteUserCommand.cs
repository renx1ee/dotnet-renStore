namespace RenStore.Identity.Application.Features.User.Commands.DeleteUser;

public sealed record DeleteUserCommand(Guid UserId) : IRequest;