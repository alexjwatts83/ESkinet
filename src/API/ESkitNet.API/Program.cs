var builder = WebApplication.CreateBuilder(args);

/******************************************/
/* Add Services to container and other DI */
/******************************************/

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true)
            .AllowAnyHeader());
});

builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);

var app = builder.Build();

/****************************************/
/* Configure the HTTP request pipeline. */
/****************************************/
app.UseCors("CorsPolicy");

app.UseApiServices();

if (app.Environment.IsDevelopment())
    await app.InitialiseDatabaseAsync();

app.Run();
