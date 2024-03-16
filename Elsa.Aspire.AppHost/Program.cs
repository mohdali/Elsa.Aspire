var builder = DistributedApplication.CreateBuilder(args);

var postgrespw = builder.Configuration["postgrespassword"];

var postgresdb = builder.AddPostgres("pg", password: postgrespw)
    .WithVolumeMount("postgres-data", "/var/lib/postgresql/data")
    .AddDatabase("elsadb");

var messaging = builder.AddRabbitMQ("messaging");

var server = builder.AddProject<Projects.Elsa_Aspire_Server>("elsaserver")
        .WithReplicas(2)
        .WithReference(postgresdb)
        .WithReference(messaging);

builder.AddProject<Projects.Elsa_Aspire_Studio>("elsastudio")
    .WithReference(server);

builder.Build().Run();
