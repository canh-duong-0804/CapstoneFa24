using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HealthTrackingManageAPI.Authorize
{
    public class RoleLessThanOrEqualToAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly int _maxRole;

        public RoleLessThanOrEqualToAttribute(int maxRole)
        {
            _maxRole = maxRole;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var roleClaim = user.FindFirst("Role"); // Assuming "Role" is the claim type for role
                if (roleClaim != null && int.TryParse(roleClaim.Value, out int roleValue))
                {
                    if (roleValue <= _maxRole)
                    {
                        return; // Authorized
                    }
                }
            }

            // Unauthorized response
            context.Result = new Microsoft.AspNetCore.Mvc.ForbidResult();
        }
    }
}
