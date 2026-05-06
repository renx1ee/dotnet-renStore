namespace RenStore.Identity.Application.Features.User.Commands.ChangeEmail;

public sealed record ChangeEmailCommand(Guid UserId, string NewEmail) : IRequest;