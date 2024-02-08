# Elsa Aspire
This sample application demonstrates a configuration of [Elsa Workflows](https://github.com/elsa-workflows/elsa-core) using .NET Aspire.

The application consists of:

- [Elsa Studio](https://github.com/elsa-workflows/elsa-studio)
- [Elsa Server](https://github.com/elsa-workflows/elsa-core) Running in two node using `MassTransitDispatcher` for distributed management
- PostgreSQL database for persistence
- RabbitMQ for server node communication

//TODOS
- Configure YARP for HTTP Activities and SignalR Sticky Sessions