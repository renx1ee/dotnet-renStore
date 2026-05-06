using Microsoft.Extensions.Logging;
using RenStore.Identity.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Identity.Application.Features.User.Commands.RemoveRole;

internal sealed class RemoveRoleCommandHandler(
    IApplicationUserRepository userRepository,
    ILogger<RemoveRoleCommandHandler> logger)
    : IRequestHandler<RemoveRoleCommand>
{
    public async Task Handle(
        RemoveRoleCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(request.UserId, cancellationToken)
                   ?? throw new NotFoundException(typeof(ApplicationUser), request.UserId);

        user.RemoveRole(request.RoleId, DateTimeOffset.UtcNow);
        await userRepository.SaveAsync(user, cancellationToken);

        logger.LogInformation(
            "Role removed. UserId={UserId} RoleId={RoleId}",
            request.UserId, request.RoleId);
    }
}