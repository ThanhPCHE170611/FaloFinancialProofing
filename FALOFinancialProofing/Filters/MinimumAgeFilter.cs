using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace FALOFinancialProofing.Filters
{
    public class MinimumAgeFilter : IAuthorizationFilter
    {
        private readonly int _minimumAge;

        public MinimumAgeFilter(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var dateOfBirthClaim = user.FindFirst(c => c.Type == ClaimTypes.DateOfBirth);
            if(dateOfBirthClaim == null)
            {
                context.Result = new ForbidResult();
                return;
            }
            var dateOfBirth = Convert.ToDateTime(dateOfBirthClaim.Value);
            var age = DateTime.Today.Year - dateOfBirth.Year;
            if(dateOfBirth.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }
            if(age < _minimumAge)
            {
                context.Result = new ForbidResult();
                return;
            }

        }
    }
}
