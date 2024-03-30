using Elsa.Studio.Core.BlazorServer.Extensions;
using Elsa.Studio.Dashboard.Extensions;
using Elsa.Studio.Extensions;
using Elsa.Studio.Login.BlazorServer.Extensions;
using Elsa.Studio.Login.HttpMessageHandlers;
using Elsa.Studio.Shell.Extensions;
using Elsa.Studio.Workflows.Extensions;
using Elsa.Studio.Workflows.Designer.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.Extensions.ServiceDiscovery.Http;
using Microsoft.Extensions.ServiceDiscovery.Abstractions;
using Microsoft.Extensions.ServiceDiscovery;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.AddServiceDefaults();

// Register Razor services.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor(options =>
{
    // Register the root components.
    options.RootComponents.RegisterCustomElsaStudioElements();
});

// Register shell services and modules.
builder.Services.AddCore();
builder.Services.AddShell(options => configuration.GetSection("Shell").Bind(options));
builder.Services.AddRemoteBackend(
    elsaClient => elsaClient.AuthenticationHandler = typeof(AuthenticatingApiHttpMessageHandler),
    options => configuration.GetSection("Backend").Bind(options));
builder.Services.AddLoginModule();
builder.Services.AddDashboardModule();
builder.Services.AddWorkflowsModule();

builder.Services.AddSingleton<IConfigureOptions<HttpConnectionOptions>, ServiceDiscoveryHttpOptionsConfigurer>(provider =>
{
    var timeProvider = provider.GetService<TimeProvider>() ?? TimeProvider.System;
    var selectorProvider = provider.GetRequiredService<IServiceEndPointSelectorProvider>();
    var resolverProvider = provider.GetRequiredService<ServiceEndPointResolverFactory>();
    var registry = new HttpServiceEndPointResolver(resolverProvider, selectorProvider, timeProvider);

    return new ServiceDiscoveryHttpOptionsConfigurer(registry);
});

// Configure SignalR.
builder.Services.AddSignalR(options =>
{
    // Set MaximumReceiveMessageSize to handle large workflows.
    options.MaximumReceiveMessageSize = 5 * 1024 * 1000; // 5MB
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

public class ServiceDiscoveryHttpOptionsConfigurer : IConfigureOptions<HttpConnectionOptions>
{
    private readonly HttpServiceEndPointResolver _registry;

    public ServiceDiscoveryHttpOptionsConfigurer(HttpServiceEndPointResolver registry)
    {
        _registry = registry;
    }

    public void Configure(HttpConnectionOptions options)
    {
        options.HttpMessageHandlerFactory = handler => new ResolvingHttpDelegatingHandler(_registry, handler);
    }
}
