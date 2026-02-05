var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("env");

var postgres = builder
    .AddPostgres("pg")
    .WithDataVolume()
    .WithPgAdmin();

var db = postgres.AddDatabase("mercator");

builder.AddProject<Projects.Mercator_Bootstrapper>("mercator-bootstrapper")
    .WithHttpHealthCheck("/health")
    .WaitFor(db)
    .WithReference(db);

builder.Build().Run();
