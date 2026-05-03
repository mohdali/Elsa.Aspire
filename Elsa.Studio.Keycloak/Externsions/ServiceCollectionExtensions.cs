using Elsa.Studio.Contracts;
using Elsa.Studio.Keycloak.ComponentProviders;
using Elsa.Studio.Keycloak.HttpMessageHandlers;
using Elsa.Studio.Keycloak.Services;
using Elsa.Studio.Login.Extensions;
using Elsa.Studio.Login.BlazorServer.Services;
using Elsa.Studio.Login.BlazorServer.Extensions;
using Elsa.Studio.Login.Contracts;
using Elsa.Studio.Login.HttpMessageHandlers;
using Elsa.Studio.Login.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Elsa.Studio.Keycloak.Externsions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKeycloakModule(this IServiceCollection services, IConfiguration configuration)
    {

        var oidcScheme = OpenIdConnectDefaults.AuthenticationScheme;
        var keycloakEndpoint = configuration["services:keycloak:http:0"]?.TrimEnd('/') ?? "http://keycloak";
        var keycloakRealmEndpoint = $"{keycloakEndpoint}/realms/Elsa";

        services
            .AddLoginModule()
            .UseOpenIdConnect(options =>
            {
                options.AuthEndpoint = $"{keycloakRealmEndpoint}/protocol/openid-connect/auth";
                options.TokenEndpoint = $"{keycloakRealmEndpoint}/protocol/openid-connect/token";
                options.EndSessionEndpoint = $"{keycloakRealmEndpoint}/protocol/openid-connect/logout";
                options.ClientId = "ElsaServer";
                options.Scopes = ["openid", "profile"];
            });

        services
            .AddScoped<IFeature, Feature>()
            .AddHttpContextAccessor()
            .AddScoped<KeycloakAuthorizationHandler>()
            .AddScoped<AuthenticatingApiHttpMessageHandler>()
            .AddScoped<AuthenticationStateProvider, KeycloakAuthenticationStateProvider>()
            .AddScoped<IUnauthorizedComponentProvider, RedirectToKeycloakComponentProvider>()
            //.AddScoped<ICredentialsValidator, DefaultCredentialsValidator>()            
            .AddSingleton<IJwtParser, BlazorServerJwtParser>()
            .AddScoped<IAuthenticationProvider, JwtAuthenticationProvider>()
            .AddScoped<IJwtAccessor, KeycloakJwtAccessor>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = oidcScheme;
            })
            .AddKeycloakOpenIdConnect("keycloak", realm: "Elsa", oidcScheme, options =>
            {
                options.ClientId = "ElsaServer";
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.Scope.Add("profile");
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
                options.SaveTokens = true;
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

        services.AddCascadingAuthenticationState();

        return services;
    }
}
