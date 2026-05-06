using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Identity.Application.Abstractions.Queries;
using RenStore.Identity.Application.Abstractions.Services;
using RenStore.Identity.Domain.Enums;
using RenStore.Identity.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Identity.Application.Features.User.Commands.Login;

internal sealed class LoginUserCommandHandler(
    IApplicationUserRepository userRepository,
    IApplicationUserQuery      userQuery,
    IPasswordHasher            passwordHasher,
    ITokenService              tokenService,
    ILogger<LoginUserCommandHandler> logger)
    : IRequestHandler<LoginUserCommand, LoginResult>
{
    public async Task<LoginResult> Handle(
        LoginUserCommand  request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. Email={Email}",
            nameof(LoginUserCommand), request.Email);

        var readModel = await userQuery.FindByEmailAsync(request.Email, cancellationToken)
                        ?? throw new UnauthorizedException("Invalid credentials.");

        if (readModel.Status == ApplicationUserStatus.IsDeleted)
            throw new UnauthorizedException("Account is deleted.");

        if (readModel.Status == ApplicationUserStatus.Locked &&
            readModel.LockoutEnd > DateTimeOffset.UtcNow)
            throw new UnauthorizedException(
                $"Account is locked until {readModel.LockoutEnd:u}.");

        var user = await userRepository.GetAsync(readModel.Id, cancellationToken)
                   ?? throw new NotFoundException(typeof(ApplicationUser), readModel.Id);

        var now = DateTimeOffset.UtcNow;

        if (!passwordHasher.Verify(request.Password, readModel.PasswordHash))
        {
            user.LoginFailed(now);
            await userRepository.SaveAsync(user, cancellationToken);
            throw new UnauthorizedException("Invalid credentials.");
        }

        user.LoginSucceeded(now);
        await userRepository.SaveAsync(user, cancellationToken);

        var accessToken  = tokenService.GenerateAccessToken(readModel);
        var refreshToken = tokenService.GenerateRefreshToken();

        logger.LogInformation(
            "User logged in. UserId={UserId}", readModel.Id);

        return new LoginResult(readModel.Id, accessToken, refreshToken);
    }
}