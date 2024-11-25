using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.FALOHomePage.Controllers
{
    public class HomepageController : Controller
    {
        // GET: HomepageController
        public ActionResult Index()
        {
            return View();
        }

        // GET: HomepageController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomepageController/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult RedirectToProjectPage()
        {
            // Redirecting to the Index action of ProjectpageController
            return RedirectToAction("Index", "Projectpage");
        }

        // POST: HomepageController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomepageController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomepageController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomepageController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomepageController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
