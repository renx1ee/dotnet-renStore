using MediatR;

namespace RenStore.Identity.Application.Features.User.Commands.ChangePassword;

public sealed record ChangePasswordCommand(
    Guid   UserId,
    string OldPassword,
    string NewPassword) 
    : IRequest;