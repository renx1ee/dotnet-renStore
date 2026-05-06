using Microsoft.Extensions.Logging;
using RenStore.Identity.Application.Abstractions.Queries;
using RenStore.Identity.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Identity.Application.Features.User.Commands.ChangeEmail;

internal sealed class ChangeEmailCommandHandler(
    IApplicationUserRepository userRepository,
    IApplicationUserQuery      userQuery,
    ILogger<ChangeEmailCommandHandler> logger)
    : IRequestHandler<ChangeEmailCommand>
{
    public async Task Handle(
        ChangeEmailCommand request,
        CancellationToken  cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. UserId={UserId}",
            nameof(ChangeEmailCommand), request.UserId);

        // Проверка уникальности нового email
        var existing = await userQuery.FindByEmailAsync(request.NewEmail, cancellationToken);
        if (existing is not null && existing.Id != request.UserId)
            throw new ConcurrencyException("Email is already taken.");

        var user = await userRepository.GetAsync(request.UserId, cancellationToken)
                   ?? throw new NotFoundException(typeof(ApplicationUser), request.UserId);

        user.ChangeEmail(request.NewEmail, DateTimeOffset.UtcNow);
        await userRepository.SaveAsync(user, cancellationToken);

        logger.LogInformation(
            "Email change requested. UserId={UserId} NewEmail={Email}",
            request.UserId, request.NewEmail);
    }
}