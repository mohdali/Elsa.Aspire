using Elsa.Studio.Login.Contracts;
using Microsoft.AspNetCore.Components;

namespace Elsa.Studio.Keycloak.Services;

public class KeycloakEndSessionService(NavigationManager navigationManager) : IEndSessionService
{
    public Task LogoutAsync()
    {
        navigationManager.NavigateTo("/authentication/logout", forceLoad: true);
        return Task.CompletedTask;
    }
}
