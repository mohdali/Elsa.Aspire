using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

public class KeycloakClaimsTransofrmation : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var claimsIdentity = new ClaimsIdentity();


        if (principal.Identity.IsAuthenticated)
        {
            var claimType = "permissions";

            claimsIdentity.AddClaim(new Claim(claimType, "*"));

            principal.AddIdentity(claimsIdentity);

        }

        return Task.FromResult(principal);
    }
}
