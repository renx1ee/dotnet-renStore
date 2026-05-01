using System.Text.Json.Serialization;
using Asp.Versioning;
using RenStore.Payment.Application;
using RenStore.Payment.Application.Abstractions.Services;
using RenStore.Payment.Application.Services;
using RenStore.Payment.Messaging.Extensions;
using RenStore.Payment.Persistence;
using RenStore.Payment.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddPaymentPersistence(builder.Configuration);
builder.Services.AddPaymentMessaging(builder.Configuration);
builder.Services.AddPaymentApplication();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<IPaymentProviderService, YooKassaPaymentService>();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions
            .Converters
            .Add(new JsonStringEnumConverter());
    });

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = false;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
