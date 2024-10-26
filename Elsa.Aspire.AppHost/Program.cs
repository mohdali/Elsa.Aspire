using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
                      .WithDataVolume(isReadOnly: false);

var postgresdb = postgres.AddDatabase("elsadb");

var messaging = builder.AddRabbitMQ("messaging");

var server = builder.AddProject<Projects.Elsa_Aspire_Server>("elsaserver")
        .WithReplicas(2)
        .WithReference(postgresdb)
        .WithReference(messaging);

builder.AddProject<Projects.Elsa_Aspire_Studio>("elsastudio")
    .WithReference(server);

builder.Build().Run();
