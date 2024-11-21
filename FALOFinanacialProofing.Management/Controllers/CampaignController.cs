using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FALOFinanacialProofing.Management.Controllers
{
    public class CampaignController : Controller
    {
        public ActionResult CampaignRequestManagement()
        {
            return View();
        }
        public ActionResult CampaignManagement()
        {
            return View();
        }
        public ActionResult CampaignDetail()
        {
            return View();
        }
        public ActionResult CampaignDetail_PMB()
        {
            return View();
        }
        public ActionResult CampaignCreate()
        {
            return View();
        }
        public ActionResult CampaignUpdateProcessRequest()
        {
            return View();
        }
        public ActionResult Member()
        {
            return View();
        }
        public ActionResult Member_PMB()
        {
            return View();
        }
        public ActionResult MemberAdd()
        {
            return View();
        }
        public ActionResult CampaignRequestDetail()
        {
            return View();
        }
        public ActionResult CampaignUpdateProcessManagement()
        {
            return View();
        }
        public ActionResult CampaignUpdateProcessRequestDetail()
        {
            return View();
        }
        public ActionResult MemberDetail()
        {
            return View();
        }


        // GET: CampaignController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CampaignController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CampaignController/Create
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

        // GET: CampaignController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CampaignController/Edit/5
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

        // GET: CampaignController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CampaignController/Delete/5
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
