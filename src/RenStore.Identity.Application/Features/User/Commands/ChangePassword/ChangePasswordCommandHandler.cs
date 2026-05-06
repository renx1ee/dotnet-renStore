using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Identity.Application.Abstractions.Queries;
using RenStore.Identity.Application.Abstractions.Services;
using RenStore.Identity.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Identity.Application.Features.User.Commands.ChangePassword;

internal sealed class ChangePasswordCommandHandler(
    IApplicationUserRepository userRepository,
    IApplicationUserQuery      userQuery,
    IPasswordHasher            passwordHasher,
    ILogger<ChangePasswordCommandHandler> logger)
    : IRequestHandler<ChangePasswordCommand>
{
    public async Task Handle(
        ChangePasswordCommand request,
        CancellationToken     cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. UserId={UserId}",
            nameof(ChangePasswordCommand), request.UserId);

        var readModel = await userQuery.FindByIdAsync(request.UserId, cancellationToken)
                        ?? throw new NotFoundException(typeof(ApplicationUser), request.UserId);

        if (!passwordHasher.Verify(request.OldPassword, readModel.PasswordHash))
            throw new UnauthorizedException("Old password is incorrect.");

        var user = await userRepository.GetAsync(request.UserId, cancellationToken)!;

        var newHash = passwordHasher.Hash(request.NewPassword);

        user.ChangePassword(newHash, DateTimeOffset.UtcNow);
        await userRepository.SaveAsync(user, cancellationToken);

        logger.LogInformation("Password changed. UserId={UserId}", request.UserId);
    }
}