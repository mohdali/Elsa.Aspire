# Elsa Aspire
This sample application demonstrates a configuration of [Elsa Workflows](https://v3.elsaworkflows.io/) using .NET Aspire.

The application consists of:

- [Elsa Studio](https://github.com/elsa-workflows/elsa-studio)
- [Elsa Server](https://github.com/elsa-workflows/elsa-core) Running in two node using `MassTransitDispatcher` for distributed management
- PostgreSQL database for persistence
- RabbitMQ for server node communication


With the latest Aspire 4 preview, database passwords need to be consistent between appl launches. Set the Postgres password in the secrets as below.
(See [Persist data using volumes](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/persist-data-volumes))
```
dotnet user-secrets set postgrespassword <password>
```