using Mercator.Bootstrapper.Extensions;
using Mercator.Bootstrapper.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddMercatorModules(builder.Configuration);
builder.Services.AddGlobalConfigurations(builder.Configuration);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

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

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseMercatorAuth();

app.UseCors();

app.MapMercatorModules();

app.Run();
