using FALOFinancialProofing.Filters.RoleFilters;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.Attributes.RoleAttributes
{
    public class RoleAttribute : TypeFilterAttribute
    {
        public RoleAttribute(string userRole) : base(typeof(RoleFilter))
        {
            Arguments = new object[] { userRole };
        }
    }
}
