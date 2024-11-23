using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
                      .WithDataVolume(isReadOnly: false)
                      .WithLifetime(ContainerLifetime.Persistent);

var postgresdb = postgres.AddDatabase("elsadb");

var messaging = builder.AddRabbitMQ("messaging")
                      .WithLifetime(ContainerLifetime.Persistent);

var server = builder.AddProject<Projects.Elsa_Aspire_Server>("elsaserver")
        .WithReplicas(2)
        .WithHttpHealthCheck("/health")
        .WithReference(postgresdb)
        .WithReference(messaging)
        .WaitFor(postgresdb)
        .WaitFor(messaging);

builder.AddProject<Projects.Elsa_Aspire_Studio>("elsastudio")
    .WithReference(server)
    .WaitFor(server);

builder.Build().Run();
