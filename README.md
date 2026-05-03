# Elsa Aspire
This sample application demonstrates a configuration of [Elsa Workflows](https://v3.elsaworkflows.io/) using .NET Aspire.

The application consists of:

- [Elsa Studio](https://github.com/elsa-workflows/elsa-studio)
- [Elsa Server](https://github.com/elsa-workflows/elsa-core) Running in two node using distributed runtime
- PostgreSQL database for persistence
- RabbitMQ for server node communication
- Keycloak for authentication

## Prerequisites

- .NET 9 SDK
- Docker

This repository includes a `global.json` that pins the SDK to .NET 9. This is required because Aspire 9.1 is used by the AppHost. If multiple SDKs are installed, verify that the repository selects .NET 9:

```
dotnet --version
```

## Running locally

Run the Aspire AppHost:

```
dotnet run --project ./Elsa.Aspire.AppHost/Elsa.Aspire.AppHost.csproj
```

Open the Aspire dashboard URL printed by the AppHost. From there, open the Elsa Studio resource.

## Keycloak

Keycloak is configured with sample realm and client for the application. The realm and client configuration can be found in `Realms` directory under `Elsa.Aspire.AppHost`.

When redirected to the Keycloak login page for the first time, register a new user and use that user for subsequent logins.

The `ElsaServer` Keycloak client is configured to issue access tokens with the `ElsaServer` audience. Elsa Server validates this audience before accepting Studio API calls.

Elsa uses FastEndpoints with Permissions for authorization. For demo purposes, an `IClaimsTransformation` is used to add `*` permission to authenticated users.

## Postgres

Database passwords need to be consistent between app launches. Set the Postgres password in the secrets as below.
(See [Persist data using volumes](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/persist-data-volumes))
```
dotnet user-secrets set "Parameters:postgres-password" <password>
```

## K8s Deployment

[Aspirate](https://github.com/prom3theu5/aspirational-manifests) can be used for Kubernetes deployment. A customized manifest is included for easy deployment.

Inside `Elsa.Aspire.AppHost` Porject, run below command to generate deployment files:

```
aspirate generate
```
Followed by:
```
aspirate apply
```
