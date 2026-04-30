using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RenStore.Payment.Persistence;

public class PaymentDbContextFactory : IDesignTimeDbContextFactory<PaymentDbContext>
{
    public PaymentDbContext CreateDbContext(string[] args)
    {
        // macOS/linux: export CONNECTION_STRING="Server=localhost;Port=5432;DataBase=renstore_catalog; User Id=re;Password=postgres ;Include Error Detail=True"
        var connectionString =
            Environment.GetEnvironmentVariable("CONNECTION_STRING")
            ?? "Server=localhost;Port=5432;DataBase=renstore_payment; User Id=re;Password=postgres ;Include Error Detail=True";
        
        var optionsBuilder = new DbContextOptionsBuilder<PaymentDbContext>();
        
        optionsBuilder.UseNpgsql(connectionString);

        return new PaymentDbContext(optionsBuilder.Options);
    }
}