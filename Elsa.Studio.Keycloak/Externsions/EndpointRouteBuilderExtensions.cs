using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Elsa.Studio.Keycloak.Externsions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointConventionBuilder MapKeycloakLogin(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("authentication");

        group.MapGet("/login", () => TypedResults.Challenge(new AuthenticationProperties { RedirectUri = "/" }))
            .AllowAnonymous();

        group.MapGet("/logout", () => TypedResults.SignOut(new AuthenticationProperties { RedirectUri = "/" },
            [CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme]));

        return group;
    }
}
