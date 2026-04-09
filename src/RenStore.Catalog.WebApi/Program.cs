var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCatalogPersistence(builder.Configuration);
builder.Services.AddCatalogApplication();
builder.Services.AddCatalogMessaging(builder.Configuration);

builder.Services.AddHttpContextAccessor();
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

builder.Services.AddMassTransit(x =>
{
    /*x.AddConsumer<VariantCreatedConsumer>();*/
    /*x.AddConsumer<VariantDeletedConsumer>();*/
            
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(
            host: builder.Configuration["RabbitMQ:Host"], 
            virtualHost: builder.Configuration["RabbitMQ:VHost"], 
            configure: h =>
            {
                h.Username(builder.Configuration["RabbitMQ:Username"]!);
                h.Password(builder.Configuration["RabbitMQ:Password"]!);
            });
                
        /*cfg.ReceiveEndpoint("inventory.variant.created", e =>
        {
            e.ConfigureConsumer<VariantSizeCreatedEvent>(context);
        });*/
                
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.MapControllers();

app.Run();