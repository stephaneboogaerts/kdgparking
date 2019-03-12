using kdgparking.BL;
using kdgparking.BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace kdgparking.Controllers
{
    public class ContractController : Controller
    {
        IContractManager cMgr = new ContractManager();
        // Change status badge, view disabled, assign badge
        // GET: Contract
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Assign(int holderId, int badgeId)
        {
            // TODO :
            return View();
        }

        public ActionResult Activate(int badgeId)
        {
            Badge badge = cMgr.GetBadge(badgeId);
            cMgr.ChangeBadgeStatusToActive(badge);
            return RedirectToAction("LijstActive", "Holder");
        }

        public ActionResult Lost(int badgeId)
        {
            Badge badge = cMgr.GetBadge(badgeId);
            cMgr.ChangeBadgeStatusToLost(badge);
            return RedirectToAction("LijstActive", "Holder");
        }

        public ActionResult Disable(int badgeId)
        {
            Badge badge = cMgr.GetBadge(badgeId);
            cMgr.ChangeBadgeStatusToDisabled(badge);
            return RedirectToAction("LijstActive", "Holder");
        }
    }
}