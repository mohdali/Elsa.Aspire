using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var pgpassword = builder.AddParameter("postgrespassword", true);

var postgresdb = builder.AddPostgres("pg", password: pgpassword)
    .WithVolume("postgres-data", "/var/lib/postgresql/data")
    .AddDatabase("elsadb");

var messaging = builder.AddRabbitMQ("messaging");

var server = builder.AddProject<Projects.Elsa_Aspire_Server>("elsaserver")
        .WithReplicas(2)
        .WithReference(postgresdb)
        .WithReference(messaging);

builder.AddProject<Projects.Elsa_Aspire_Studio>("elsastudio")
    .WithReference(server);

builder.Build().Run();
