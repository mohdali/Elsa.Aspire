using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;

namespace Elsa.Studio.Keycloak.Externsions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointConventionBuilder MapKeycloakLogin(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("authentication");

        group.MapGet("/login", () => TypedResults.Challenge(new AuthenticationProperties { RedirectUri = "/" }))
            .AllowAnonymous();

        group.MapGet("/logout", async (HttpContext httpContext, IConfiguration configuration) =>
        {
            var idToken = await httpContext.GetTokenAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectParameterNames.IdToken);

            if (!IsCurrentIssuer(idToken, configuration))
            {
                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Results.LocalRedirect("/");
            }

            return Results.SignOut(
                new AuthenticationProperties { RedirectUri = "/" },
                [CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme]);
        });

        return group;
    }

    private static bool IsCurrentIssuer(string? idToken, IConfiguration configuration)
    {
        if (string.IsNullOrWhiteSpace(idToken))
            return false;

        var keycloakEndpoint = configuration["services:keycloak:http:0"]?.TrimEnd('/');
        if (string.IsNullOrWhiteSpace(keycloakEndpoint))
            return true;

        var expectedIssuer = $"{keycloakEndpoint}/realms/Elsa";
        try
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(idToken);
            return string.Equals(token.Issuer, expectedIssuer, StringComparison.Ordinal);
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
}
