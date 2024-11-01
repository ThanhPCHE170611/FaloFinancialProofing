using FALOFinancialProofing.FALOHomePage.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinancialProofing.FALOHomePage.Controllers
{
    
    public class DonationController : Controller
    {
        // GET: DonationController
        private readonly TransactionPollingDirect _transactionPollingDirect;
        public DonationController(TransactionPollingDirect transactionPollingDirect)
        {
            _transactionPollingDirect = transactionPollingDirect;
        }
        public async Task<IActionResult> Index()
        {
            var transactions = await _transactionPollingDirect.GetTransactionAsync();
            return View(transactions);
        }

        // GET: DonationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DonationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DonationController/Create
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

        // GET: DonationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DonationController/Edit/5
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

        // GET: DonationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DonationController/Delete/5
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
