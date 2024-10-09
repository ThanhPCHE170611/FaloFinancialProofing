using Microsoft.AspNetCore.Authorization;

namespace FALOFinancialProofing.Attributes
{
    public class MinimumAgeAuthorizeAttribute : AuthorizeAttribute
    {
        //public MinimumAgeAuthorizeAttribute(int age,string policy,string a)
        //{
        //    Policy = $"MinimumAge{age}";
            
        //}
        public MinimumAgeAuthorizeAttribute(int age)
        {
            Policy = $"MinimumAge{age}";
        }
    }   
}
