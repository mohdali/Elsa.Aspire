using Elsa.Studio.Login.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.Studio.Keycloak.Services
{
    public class KeycloakJwtAccessor(IHttpContextAccessor httpContextAccessor) : IJwtAccessor
    {
        public async ValueTask<string?> ReadTokenAsync(string name)
        {
            var httpContext = httpContextAccessor.HttpContext ??
            throw new InvalidOperationException("No HttpContext available from the IHttpContextAccessor!");

            return await httpContext.GetTokenAsync(name);           
        }

        public ValueTask WriteTokenAsync(string name, string token)
        {
            throw new NotImplementedException();
        }
    }
}
