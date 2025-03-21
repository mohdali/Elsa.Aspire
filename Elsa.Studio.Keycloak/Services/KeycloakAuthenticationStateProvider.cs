using Elsa.Studio.Extensions;
using Elsa.Studio.Login.Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.Studio.Keycloak.Services
{
    public class KeycloakAuthenticationStateProvider(IJwtAccessor jwtAccessor, IJwtParser jwtParser) : AuthenticationStateProvider
    {
        /// <inheritdoc />
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var authToken = await jwtAccessor.ReadTokenAsync("access_token");

            if (string.IsNullOrEmpty(authToken))
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var claims = jwtParser.Parse(authToken).ToList();
            var isExpired = claims.IsExpired();

            // If the token has expired, return an empty authentication state.
            if (isExpired)
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            // Otherwise, return the authentication state.
            var identity = new ClaimsIdentity(claims, "jwt", "name", "role");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        /// <summary>
        /// Notifies the authentication state has changed.
        /// </summary>
        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
