var builder = WebApplication.CreateBuilder(args);

/******************************************/
/* Add Services to container and other DI */
/******************************************/

builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);

var app = builder.Build();

/****************************************/
/* Configure the HTTP request pipeline. */
/****************************************/
app.UseApiServices();

if (app.Environment.IsDevelopment())
    await app.InitialiseDatabaseAsync();

app.Run();
