using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.Studio.Keycloak.Components;

public class RedirectToKeycloakLogin : ComponentBase
{
    /// <summary>
    /// Gets or sets the <see cref="NavigationManager"/>.
    /// </summary>
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;

    /// <inheritdoc />
    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        NavigationManager.NavigateTo("authentication/login", true);
        return Task.CompletedTask;
    }
}
