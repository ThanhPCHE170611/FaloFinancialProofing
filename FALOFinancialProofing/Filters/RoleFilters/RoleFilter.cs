using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace FALOFinancialProofing.Filters.RoleFilters
{
    public class RoleFilter : IAuthorizationFilter
    {
        private readonly string _role;
        public RoleFilter(string role)
        {
            _role = role;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            bool checkUserLegit = user.IsInRole(_role);
            if (!checkUserLegit)
            {
                context.Result = new ForbidResult();
                return;
            }
            //bool checkUserLegit = user.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == _role);
        }
    }
}
