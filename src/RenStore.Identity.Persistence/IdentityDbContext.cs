using Microsoft.EntityFrameworkCore;
using RenStore.Identity.Domain.ReadModels;
using RenStore.Identity.Persistence.EntityTypeConfigurations;
using RenStore.Identity.Persistence.EventStore;
using RenStore.Identity.Persistence.Outbox;

namespace RenStore.Identity.Persistence;

public sealed class IdentityDbContext(DbContextOptions<IdentityDbContext> options)
    : DbContext(options)
{
    public DbSet<ApplicationUserReadModel>          Users                     => Set<ApplicationUserReadModel>();
    public DbSet<UserRoleReadModel>                 UserRoles                 => Set<UserRoleReadModel>();
    public DbSet<RoleReadModel>                     Roles                     => Set<RoleReadModel>();
    public DbSet<PendingEmailVerificationReadModel> PendingEmailVerifications => Set<PendingEmailVerificationReadModel>();
    public DbSet<EventEntity>                       Events                    => Set<EventEntity>();
    public DbSet<OutboxMessage>                     OutboxMessages            => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OutboxConfiguration());
        modelBuilder.ApplyConfiguration(new EventEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new PendingEmailVerificationConfiguration());
    }
}