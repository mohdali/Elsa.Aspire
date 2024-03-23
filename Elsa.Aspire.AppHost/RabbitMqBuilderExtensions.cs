using System.Net.Sockets;

namespace Elsa.Aspire.AppHost
{
    internal static class RabbitMqBuilderExtensions
    {
        public static IResourceBuilder<RabbitMQServerResource> AddRabbitMQ(this IDistributedApplicationBuilder builder, string name, string password, int? port = null)
        {
            var rabbitMq = new RabbitMQServerResource(name, password);
            return builder.AddResource(rabbitMq)
                           .WithAnnotation(new EndpointAnnotation(ProtocolType.Tcp, port: port, containerPort: 5672))
                           .WithAnnotation(new ContainerImageAnnotation { Image = "rabbitmq", Tag = "3" })
                           .WithEnvironment("RABBITMQ_DEFAULT_USER", "guest")
                           .WithEnvironment(context =>
                           {
                               if (context.ExecutionContext.IsPublishMode)
                               {
                                   context.EnvironmentVariables.Add("RABBITMQ_DEFAULT_PASS", $"{{{rabbitMq.Name}.inputs.password}}");
                               }
                               else
                               {
                                   context.EnvironmentVariables.Add("RABBITMQ_DEFAULT_PASS", rabbitMq.Password);
                               }
                           })
                           .PublishAsContainer();
        }
    }
}
