using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinanacialProofing.Management.Controllers
{
    public class AccountingController : Controller
    {
        // GET: AccountingController
        public ActionResult DebtManagement()
        {
            return View();
        }
        public ActionResult AccountingBookCampaign()
        {
            return View();
        }
        public ActionResult AccountingBookProject()
        {
            return View();
        }
        public ActionResult AccountingBookCampaign_PMB()
        {
            return View();
        }

        // GET: AccountingController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountingController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountingController/Create
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

        // GET: AccountingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountingController/Edit/5
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

        // GET: AccountingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountingController/Delete/5
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
