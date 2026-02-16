using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RenStore.Domain.Entities;
using RenStore.Identity.Domain.Entities;

namespace RenStore.Identity.DuendeServer.WebAPI.Data.EntityConfigurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasKey(user => user.Id);
    }
}