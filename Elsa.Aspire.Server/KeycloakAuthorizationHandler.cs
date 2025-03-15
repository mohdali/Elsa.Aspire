using Microsoft.AspNetCore.Authorization;

internal class KeycloakAuthorizationHandler : IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        var currentIdentity = context.User.Identity;

        if (currentIdentity.IsAuthenticated)
        {
            foreach (var requirement in context.Requirements)
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}