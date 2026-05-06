using RenStore.Identity.Application.Abstractions.Projections;
using RenStore.Identity.Domain.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace RenStore.Identity.Persistence.Write.Projections;

internal sealed class EmailVerificationProjection(IdentityDbContext context)
    : IEmailVerificationProjection
{
    public async Task CommitAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);

    public async Task AddAsync(PendingEmailVerificationReadModel record, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(record);
        await context.PendingEmailVerifications.AddAsync(record, cancellationToken);
    }

    public async Task MarkAsUsedAsync(Guid token, CancellationToken cancellationToken)
    {
        var record = await context.PendingEmailVerifications
            .FirstOrDefaultAsync(x => x.Token == token, cancellationToken);

        if (record is not null)
            record.IsUsed = true;
    }
}