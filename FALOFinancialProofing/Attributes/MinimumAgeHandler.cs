using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FALOFinancialProofing.Attributes
{
    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                return Task.CompletedTask;
            }
            var dateOfBirth = Convert.ToDateTime(context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth)?.Value);
            int age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }
            if (age >= requirement.MinimumAge)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
