using Aspire.Hosting.Yarp;
using Aspire.Hosting.Yarp.Transforms;
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
    .WithReference(mercator);


if (builder.Environment.IsDevelopment())
{
    mercator.WithExternalHttpEndpoints();

    web.WithEnvironment("API_PROXY_TARGET", mercator.GetEndpoint("http"));
}

if (builder.ExecutionContext.IsPublishMode)
{
    var proxy = builder.AddYarp("frontend-server")
                   .WithConfiguration(c =>
                   {
                       c.AddRoute("health", mercator)
                        .WithMatchPath("/health")
                        .WithTransformPathSet("/health");
                       // Always proxy /api requests to backend
                       c.AddRoute("api/{**catch-all}", mercator)
                        .WithTransformPathRemovePrefix("/api");
                   })
                   .WithExternalHttpEndpoints()
                   .PublishWithStaticFiles(web);

    web.WithEnvironment("API_PROXY_TARGET", proxy.GetEndpoint("http"));
}

builder.Build().Run();
