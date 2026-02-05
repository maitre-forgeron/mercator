var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres("pg")
    .WithDataVolume()
    .WithPgAdmin();

var db = postgres.AddDatabase("mercator");

builder.AddProject<Projects.Mercator_Bootstrapper>("mercator-bootstrapper")
    .WaitFor(db)
    .WithReference(db);

builder.Build().Run();
