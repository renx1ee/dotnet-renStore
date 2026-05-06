using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RenStore.Identity.Persistence;

public sealed class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("CONNECTION_STRING")
            ?? "Server=localhost;Port=5432;DataBase=renstore_identity; User Id=re;Password=postgres ;Include Error Detail=True";
        
        var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
        
        optionsBuilder.UseNpgsql(connectionString);

        return new IdentityDbContext(optionsBuilder.Options);
    }
}