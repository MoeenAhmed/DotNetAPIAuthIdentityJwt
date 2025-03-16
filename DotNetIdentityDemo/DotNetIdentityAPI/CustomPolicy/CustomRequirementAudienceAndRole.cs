using Microsoft.AspNetCore.Authorization;

namespace DotNetIdentityAPI.CustomPolicy
{
    public class CustomRequirementAudienceAndRole : IAuthorizationRequirement
    {
        public readonly string _audience;
        public readonly string _role;

        public CustomRequirementAudienceAndRole(string audience, string role)
        {
            _audience = audience;
            _role = role;
        }
    }
}
