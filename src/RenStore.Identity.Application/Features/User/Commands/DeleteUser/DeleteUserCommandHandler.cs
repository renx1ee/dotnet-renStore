using Microsoft.Extensions.Logging;
using RenStore.Identity.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Identity.Application.Features.User.Commands.DeleteUser;

internal sealed class DeleteUserCommandHandler(
    IApplicationUserRepository userRepository,
    ILogger<DeleteUserCommandHandler> logger)
    : IRequestHandler<DeleteUser.DeleteUserCommand>
{
    public async Task Handle(
        DeleteUser.DeleteUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(request.UserId, cancellationToken)
                   ?? throw new NotFoundException(typeof(ApplicationUser), request.UserId);

        user.Delete(DateTimeOffset.UtcNow);
        await userRepository.SaveAsync(user, cancellationToken);

        logger.LogInformation("User deleted. UserId={UserId}", request.UserId);
    }
}