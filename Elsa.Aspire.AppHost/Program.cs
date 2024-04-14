using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var postgresdb = builder.AddPostgres("pg")
    .WithDataVolume()
    .AddDatabase("elsadb");

var messaging = builder.AddRabbitMQ("messaging");

var server = builder.AddProject<Projects.Elsa_Aspire_Server>("elsaserver")
        .WithReplicas(2)
        .WithReference(postgresdb)
        .WithReference(messaging);

builder.AddProject<Projects.Elsa_Aspire_Studio>("elsastudio")
    .WithReference(server);

builder.Build().Run();
