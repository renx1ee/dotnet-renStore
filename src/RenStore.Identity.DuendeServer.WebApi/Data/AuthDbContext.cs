using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RenStore.Domain.Entities;
using RenStore.Identity.Domain.Entities;

namespace RenStore.Identity.DuendeServer.WebAPI.Data;

public class AuthDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{ 
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*modelBuilder.Entity<ApplicationUser>(entity =>
            entity.ToTable(name: "AspNetUsers"));
        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            entity.ToTable(name: "UserRoles"));
        modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            entity.ToTable(name: "UserClaim"));
        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            entity.ToTable(name: "UserLogins"));
        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            entity.ToTable(name: "UserTokens"));
        modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            entity.ToTable(name: "RoleClaim"));*/
        
        /*modelBuilder.ApplyConfiguration(new ProductConfiguration());*/
        
        base.OnModelCreating(modelBuilder);
    }
    
    public async Task<int> SaveChangesAsync() 
    {
        return await base.SaveChangesAsync();
    }
    
    public DbSet<ApplicationUser> AspNetUsers { get; set; }
}