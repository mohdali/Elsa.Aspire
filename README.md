# Elsa Aspire
This sample application demonstrates a configuration of [Elsa Workflows](https://v3.elsaworkflows.io/) using .NET Aspire.

The application consists of:

- [Elsa Studio](https://github.com/elsa-workflows/elsa-studio)
- [Elsa Server](https://github.com/elsa-workflows/elsa-core) Running in two node using `MassTransitDispatcher` for distributed management
- PostgreSQL database for persistence
- RabbitMQ for server node communication

[Aspirate](https://github.com/prom3theu5/aspirational-manifests) can be used for Kubernetes deployment. A customized manifest is included for easy deployment.

Inside `Elsa.Aspire.AppHost` Porject, run below command to generate deployment files:

```
aspirate generate -m .\manifest.json
```
Followed by:
```
aspirate apply
```