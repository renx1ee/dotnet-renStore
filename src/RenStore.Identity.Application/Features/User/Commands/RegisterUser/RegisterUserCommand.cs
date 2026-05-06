using MediatR;

namespace RenStore.Identity.Application.Features.User.Commands.RegisterUser;

public sealed record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) 
    : IRequest<Guid>;