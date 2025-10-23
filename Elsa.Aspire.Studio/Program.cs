using Elsa.Studio.Core.BlazorServer.Extensions;
using Elsa.Studio.Dashboard.Extensions;
using Elsa.Studio.Extensions;
using Elsa.Studio.Shell.Extensions;
using Elsa.Studio.Workflows.Extensions;
using Elsa.Studio.Workflows.Designer.Extensions;
using Elsa.Studio.Login.BlazorServer.Extensions;
using Elsa.Studio.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;
using Elsa.Studio.Keycloak.HttpMessageHandlers;
using Elsa.Studio.Keycloak.Externsions;


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
var backendApiConfig = new BackendApiConfig
{
    ConfigureBackendOptions = options => configuration.GetSection("Backend").Bind(options),
    ConfigureHttpClientBuilder = options =>
    {
        options.AuthenticationHandler = typeof(KeycloakAuthorizationHandler);
    },
};

// Register shell services and modules.
builder.Services.AddCore();
builder.Services.AddShell(options => configuration.GetSection("Shell").Bind(options));
builder.Services.AddRemoteBackend(backendApiConfig);
// builder.Services.AddLoginModule(); // Commented out - causes IRefreshTokenService dependency issue
builder.Services.AddKeycloakModule();
builder.Services.AddDashboardModule();
builder.Services.AddWorkflowsModule();

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
app.UseAuthentication();
app.UseAuthorization();
app.MapBlazorHub();

app.MapKeycloakLogin();

app.MapFallbackToPage("/_Host");

app.Run();

