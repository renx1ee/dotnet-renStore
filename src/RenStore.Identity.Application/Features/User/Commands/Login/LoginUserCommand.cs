using MediatR;

namespace RenStore.Identity.Application.Features.User.Commands.Login;

public sealed record LoginUserCommand(
    string Email,
    string Password) : IRequest<LoginResult>;