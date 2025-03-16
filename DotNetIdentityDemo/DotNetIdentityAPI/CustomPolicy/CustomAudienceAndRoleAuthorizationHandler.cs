using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DotNetIdentityAPI.CustomPolicy
{
    public class CustomAudienceAndRoleAuthorizationHandler : AuthorizationHandler<CustomRequirementAudienceAndRole>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequirementAudienceAndRole requirement)
        {
            var userClaims =  context.User.Claims;

            if (context.User.Identity.IsAuthenticated &&  userClaims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value == requirement._role && userClaims.Where(c => c.Type == JwtRegisteredClaimNames.Aud).FirstOrDefault().Value == requirement._audience)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
