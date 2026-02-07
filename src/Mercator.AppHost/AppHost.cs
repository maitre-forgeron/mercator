using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("env");

var postgres = builder
    .AddPostgres("pg")
    .WithDataVolume()
    .WithPgWeb();

var db = postgres.AddDatabase("mercator");

var mercator = builder.AddProject<Projects.Mercator_Bootstrapper>("mercator-bootstrapper")
    .WithHttpHealthCheck("/health")
    .WaitFor(db)
    .WithReference(db);

var web = builder.AddViteApp("web", "../web")
    .WithReference(mercator)
    .WithEnvironment("API_PROXY_TARGET", mercator.GetEndpoint("http"));

if (builder.Environment.IsDevelopment()) 
{
    mercator.WithExternalHttpEndpoints();
}

builder.Build().Run();
