using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data;
using System.Security.Claims;

namespace FALOFinancialProofing.Filters.RoleFilters
{
    public class RoleFilter : IAuthorizationFilter
    {
        //private readonly string _role;
        //public RoleFilter(string role)
        //{
        //    _role = role;
        //}
        private readonly string[] _roles;
        public RoleFilter(params string[] roles)
        {
            _roles = roles;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            if (_roles != null && _roles.Length != 0)
            {
                bool checkUserLegit = _roles.Any(role => user.IsInRole(role));
                if (!checkUserLegit)
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
            //else
            //{
            //bool checkUserLegit = user.IsInRole(_role);
            //if (!checkUserLegit)
            //{
            //    context.Result = new ForbidResult();
            //    return;
            //}
            //}
            //bool checkUserLegit = user.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == _role);
        }
    }
}
