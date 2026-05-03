using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddKubernetesEnvironment("k8s");

var postgres = builder.AddPostgres("postgres")
         .WithDataVolume(isReadOnly: false)
         .WithLifetime(ContainerLifetime.Persistent);

var postgresdb = postgres.AddDatabase("elsadb");

var messaging = builder.AddRabbitMQ("messaging")
        .WithLifetime(ContainerLifetime.Persistent);

#pragma warning disable ASPIRECERTIFICATES001
var keycloak = builder.AddKeycloak("keycloak", port: 18080)
        .WithoutHttpsCertificate()
        .WithDataVolume()
        .WithRealmImport("./Realms")
        .WithLifetime(ContainerLifetime.Persistent);
#pragma warning restore ASPIRECERTIFICATES001

var server = builder.AddProject<Projects.Elsa_Aspire_Server>("elsaserver")
        .WithReplicas(2)
        .WithHttpHealthCheck("/health")
        .WithReference(postgresdb)
        .WithReference(messaging)
        .WithReference(keycloak)
        .WaitFor(postgresdb)
        .WaitFor(messaging)
        .WaitFor(keycloak);

builder.AddProject<Projects.Elsa_Aspire_Studio>("elsastudio")
    .WithReference(server)
    .WithReference(keycloak)
    .WaitFor(server);

builder.Build().Run();
