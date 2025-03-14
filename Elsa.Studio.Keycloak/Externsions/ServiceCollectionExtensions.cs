using Elsa.Studio.Contracts;
using Elsa.Studio.Keycloak.ComponentProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKeycloakModule(this IServiceCollection services)
    {
        return services
                .AddHttpContextAccessor()
                .AddScoped<IUnauthorizedComponentProvider, RedirectToLoginUnauthorizedComponentProvider>();
    }
}

