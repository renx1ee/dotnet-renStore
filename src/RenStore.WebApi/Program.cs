using Microsoft.EntityFrameworkCore;
using RenStore.Delivery.Persistence;
using RenStore.Persistence;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DeliveryDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

var app = builder.Build();
app.Run();