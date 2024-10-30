using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.FALOHomePage.Controllers
{
    public class ProjectpageController : Controller
    {
        // GET: ProjectpageController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ProjectpageController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProjectpageController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProjectpageController/Create
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

        // GET: ProjectpageController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProjectpageController/Edit/5
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

        // GET: ProjectpageController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProjectpageController/Delete/5
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
