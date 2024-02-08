var builder = DistributedApplication.CreateBuilder(args);

var postgresdb = builder.AddPostgresContainer("pg")    
    .WithVolumeMount("postgres-data", "/var/lib/postgresql/data", VolumeMountType.Named)
    .AddDatabase("elsadb");

var messaging = builder.AddRabbitMQContainer("messaging");

var server = builder.AddProject<Projects.Elsa_Aspire_Server>("elsaserver")
        .WithReplicas(2)
        .WithReference(postgresdb)
        .WithReference(messaging);

builder.AddProject<Projects.Elsa_Aspire_Studio>("elsastudio")
    .WithReference(server);

builder.Build().Run();
