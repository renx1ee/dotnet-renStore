using RenStore.Identity.Domain.ReadModels;

namespace RenStore.Identity.Application.Abstractions.Projections;

public interface IEmailVerificationProjection
{
    Task CommitAsync(CancellationToken cancellationToken);

    Task AddAsync(PendingEmailVerificationReadModel record, CancellationToken cancellationToken);

    Task MarkAsUsedAsync(Guid token, CancellationToken cancellationToken);
}