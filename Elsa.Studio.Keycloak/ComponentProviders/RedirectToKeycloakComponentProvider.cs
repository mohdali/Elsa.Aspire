using Elsa.Studio.Contracts;
using Elsa.Studio.Extensions;
using Elsa.Studio.Keycloak.Components;
using Microsoft.AspNetCore.Components;

namespace Elsa.Studio.Keycloak.ComponentProviders;

public class RedirectToKeycloakComponentProvider : IUnauthorizedComponentProvider
{
    public RenderFragment GetUnauthorizedComponent()
    {
        return builder => builder.CreateComponent<RedirectToKeycloakLogin>();
    }
}