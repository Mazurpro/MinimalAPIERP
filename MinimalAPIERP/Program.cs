

// FASE 1
using ERP.Api;
using MinimalAPIERP.Api;
using MinimalAPIERP.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAntiforgery();

builder.Services
    .AddCustomSqlServerDb(builder.Configuration)
    .AddCustomHealthCheck(builder.Configuration)
    .AddCustomOpenApi(builder.Configuration);

var app = builder.Build();

// FASE 2
app.MapCustomHealthCheck(builder.Configuration);

app.UseAntiforgery();

app.DatabaseInit();

app.ConfigureSwagger();

app.MapStoreApi();
app.MapProductApi();
app.MapRaincheckApi();

app.Run();
