using Mercator.Bootstrapper.Extensions;
using Mercator.Bootstrapper.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddMercatorModules(builder.Configuration);
builder.Services.AddGlobalConfigurations(builder.Configuration);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseApiExploration();

    await using var scope = app.Services.CreateAsyncScope();
    var initialiser = scope.ServiceProvider.GetRequiredService<DbInitialiser>();
    await initialiser.InitialiseAsync();
}

app.UseHttpsRedirection();

app.UseMercatorAuth();

app.MapMercatorModules();

app.Run();
