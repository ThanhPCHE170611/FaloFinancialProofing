using FALOFinancialProofing.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace FALOFinancialProofing.Attributes
{
    public class MinimumAgeAttribute : TypeFilterAttribute
    {
        //public MinimumAgeAttribute(Type type) : base(type)
        //{
        //}

        public MinimumAgeAttribute(int minimumAge) : base(typeof(MinimumAgeFilter))
        {
            Arguments = new object[] { minimumAge };
        }
    }

}
