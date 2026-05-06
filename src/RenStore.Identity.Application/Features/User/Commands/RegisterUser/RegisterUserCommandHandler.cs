using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Identity.Application.Abstractions.Queries;
using RenStore.Identity.Application.Abstractions.Services;
using RenStore.Identity.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Identity.Application.Features.User.Commands.RegisterUser;

internal sealed class RegisterUserCommandHandler(
    IApplicationUserRepository userRepository,
    IApplicationUserQuery      userQuery,
    IPasswordHasher            passwordHasher,
    ILogger<RegisterUserCommandHandler> logger)
    : IRequestHandler<RegisterUserCommand, Guid>
{
    public async Task<Guid> Handle(
        RegisterUserCommand request,
        CancellationToken   cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. Email={Email}",
            nameof(RegisterUserCommand), request.Email);

        // Проверка уникальности email
        var existing = await userQuery.FindByEmailAsync(request.Email, cancellationToken);
        
        if (existing is not null)
            throw new ConcurrencyException("User with this email already exists.");

        var hash = passwordHasher.Hash(request.Password);

        var user = ApplicationUser.Register(
            firstName:    request.FirstName,
            lastName:     request.LastName,
            email:        request.Email,
            passwordHash: hash,
            now:          DateTimeOffset.UtcNow);

        await userRepository.SaveAsync(user, cancellationToken);

        logger.LogInformation(
            "User registered. UserId={UserId} Email={Email}",
            user.Id, request.Email);

        return user.Id;
    }
}