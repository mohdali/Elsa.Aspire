using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Elsa.Aspire.Server;
using Microsoft.Extensions.Configuration;

namespace Elsa.Aspire.Tests;

public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string?>("ConnectionStrings:elsadb", "Host=localhost;Database=elsa_test;Username=postgres;Password=password"),
                new KeyValuePair<string, string?>("ConnectionStrings:messaging", "amqp://guest:guest@localhost:5672/")
            });
        });

        builder.ConfigureServices(services =>
        {
            // Just provide mock connection strings - don't try to configure Elsa for testing
            // The main configuration from Program.cs will be used but with test connection strings
        });

        builder.UseEnvironment("Testing");
    }
}