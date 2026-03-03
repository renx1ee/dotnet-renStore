using Microsoft.EntityFrameworkCore;
using RenStore.Catalog.Domain.Interfaces.Repository;
using RenStore.Delivery.Persistence;
using RenStore.Persistence.Repository.Postgresql;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");



builder.Services.AddDbContext<DeliveryDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

var app = builder.Build();
app.Run();