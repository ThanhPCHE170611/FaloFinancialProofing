using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinanacialProofing.Management.Controllers
{
    public class PaymentController : Controller
    {
        public ActionResult PaymentManagement()
        {
            return View();
        }
        public ActionResult PaymentManagement_PM()
        {
            return View();
        }
        public ActionResult PaymentCreate()
        {
            return View();
        }
        public ActionResult PaymentDetail()
        {
            return View();
        }
        public ActionResult PaymentVoucher()
        {
            return View();
        }
        public ActionResult PaymentVoucherCreate()
        {
            return View();
        }
        public ActionResult PaymentCreate_PM()
        {
            return View();
        }
        public ActionResult PaymentDetail_PM()
        {
            return View();
        }


        // GET: PaymentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PaymentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentController/Create
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

        // GET: PaymentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PaymentController/Edit/5
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

        // GET: PaymentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PaymentController/Delete/5
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
