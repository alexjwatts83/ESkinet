using ESkitNet.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

/******************************************/
/* Add Services to container and other DI */
/******************************************/

builder.Services
    .AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

/****************************************/
/* Configure the HTTP request pipeline. */
/****************************************/

app.Run();
