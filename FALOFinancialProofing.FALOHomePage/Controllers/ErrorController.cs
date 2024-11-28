using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.FALOHomePage.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error404()
        {
            return View();
        }
    }
}
