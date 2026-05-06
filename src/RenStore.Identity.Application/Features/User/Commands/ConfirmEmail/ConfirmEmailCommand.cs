/*using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Identity.Application.Abstractions.Projections;
using RenStore.Identity.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Identity.Application.Features.User.Commands.ConfirmEmail;

public sealed record ConfirmEmailCommand(Guid Token) : IRequest;

internal sealed class ConfirmEmailCommandHandler(
    IApplicationUserRepository userRepository,
    IEmailVerificationProjection verificationProjection,
    IdentityDbContext            context,
    ILogger<ConfirmEmailCommandHandler> logger)
    : IRequestHandler<ConfirmEmailCommand>
{
    public async Task Handle(
        ConfirmEmailCommand request,
        CancellationToken   cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. Token={Token}",
            nameof(ConfirmEmailCommand), request.Token);

        var verification = await context.PendingEmailVerifications
                               .FirstOrDefaultAsync(
                                   x => x.Token == request.Token && !x.IsUsed,
                                   cancellationToken)
                           ?? throw new NotFoundException("Verification token not found or already used.");

        if (verification.ExpiresAt < DateTimeOffset.UtcNow)
            throw new InvalidOperationException("Verification token has expired.");

        var user = await userRepository.GetAsync(verification.UserId, cancellationToken)
                   ?? throw new NotFoundException(nameof(ApplicationUser), verification.UserId);

        user.ConfirmEmail(DateTimeOffset.UtcNow);
        await userRepository.SaveAsync(user, cancellationToken);

        await verificationProjection.MarkAsUsedAsync(request.Token, cancellationToken);
        await verificationProjection.CommitAsync(cancellationToken);

        logger.LogInformation(
            "Email confirmed. UserId={UserId}", verification.UserId);
    }
}*/