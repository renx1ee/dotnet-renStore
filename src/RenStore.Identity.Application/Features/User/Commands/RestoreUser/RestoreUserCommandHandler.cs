using Microsoft.Extensions.Logging;
using RenStore.Identity.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Identity.Application.Features.User.Commands.RestoreUser;

internal sealed class RestoreUserCommandHandler(
    IApplicationUserRepository userRepository,
    ILogger<RestoreUserCommandHandler> logger)
    : IRequestHandler<RestoreUserCommand>
{
    public async Task Handle(
        RestoreUserCommand request,
        CancellationToken  cancellationToken)
    {
        var user = await userRepository.GetAsync(request.UserId, cancellationToken)
                   ?? throw new NotFoundException(typeof(ApplicationUser), request.UserId);

        user.Restore(DateTimeOffset.UtcNow);
        await userRepository.SaveAsync(user, cancellationToken);

        logger.LogInformation("User restored. UserId={UserId}", request.UserId);
    }
}