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

// Add redis his way
//builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
//{
//    var connectionString = builder.Configuration.GetConnectionString("Redis") ?? throw new Exception("Redis Connetion string is null");
//    var configuration = ConfigurationOptions.Parse(connectionString, true);

//    return ConnectionMultiplexer.Connect(configuration);
//});
// Add Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    //options.InstanceName = "Basket";
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
