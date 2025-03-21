# Elsa Aspire
This sample application demonstrates a configuration of [Elsa Workflows](https://v3.elsaworkflows.io/) using .NET Aspire.

The application consists of:

- [Elsa Studio](https://github.com/elsa-workflows/elsa-studio)
- [Elsa Server](https://github.com/elsa-workflows/elsa-core) Running in two node using `MassTransitDispatcher` for distributed management
- PostgreSQL database for persistence
- RabbitMQ for server node communication
- Keycloak for authentication

Keycloak is configured with sample realm and client for the application. The realm and client configuration can be found in `Realms` directory under `Elsa.Aspire.AppHost`.

When redirected to Keyclok login page for the first time, simply register a new user and use the registered users for subsequent logins.

With the latest Aspire preview, database passwords need to be consistent between app launches. Set the Postgres password in the secrets as below.
(See [Persist data using volumes](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/persist-data-volumes))
```
dotnet user-secrets set "Parameters:postgres-password" <password>
```
[Aspirate](https://github.com/prom3theu5/aspirational-manifests) can be used for Kubernetes deployment. A customized manifest is included for easy deployment.

Inside `Elsa.Aspire.AppHost` Porject, run below command to generate deployment files:

```
aspirate generate
```
Followed by:
```
aspirate apply
```