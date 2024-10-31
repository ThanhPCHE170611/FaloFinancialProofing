using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinanacialProofing.Management.Controllers
{
    public class PrepayController : Controller
    {
        public ActionResult PrepayManagement()
        {
            return View();
        }
        public ActionResult PrepayManagement_PM()
        {
            return View();
        }
        public ActionResult PrepayCreate()
        {
            return View();
        }
        public ActionResult PrepayDetail()
        {
            return View();
        }
        public ActionResult PrepayVoucher()
        {
            return View();
        }
        public ActionResult PrepayVoucherCreate()
        {
            return View();
        }



        // GET: PrepayController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PrepayController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PrepayController/Create
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

        // GET: PrepayController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PrepayController/Edit/5
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

        // GET: PrepayController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PrepayController/Delete/5
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
