using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace HealthTrackingManageAPI.Authorize
{
    public class RoleEqualToAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly int _requiredRole;

        public RoleEqualToAttribute(int requiredRole)
        {
            _requiredRole = requiredRole;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var roleClaim = user.FindFirstValue(ClaimTypes.Role);
                if (roleClaim != null && int.TryParse(roleClaim, out int roleValue))
                {
                    if (roleValue == _requiredRole)
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