using kdgparking.BL;
using kdgparking.BL.Domain;
using kdgparking.Models;
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
        IHolderManager hMgr = new HolderManager();
        // Change status badge, view disabled, assign badge
        // GET: Contract
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Assign(int holderId, string badgeId)
        {
            Holder holder = hMgr.GetHolder(holderId);
            // Gearchiveerd == false zal het juiste contract ophalen in geval van bastaande oude contracten
            // De meegegeven badgeId vanuit de view zou altijd een niet gearchiveerd contract ophalen hebben
            // Dit aangezien gearchiveerde contracten niet in de view worden vermeld
            Contract contract = holder.Contracts.FirstOrDefault(c => c.Archived == false && c.Badge.MifareSerial == badgeId);
            BadgeAssignModel model = new BadgeAssignModel()
            {
                HolderId = holder.Id,
                FirstName = holder.FirstName,
                Name = holder.Name,
                Email = holder.Email,
                MifareSerial = badgeId,
                Start = contract.StartDate,
                End = contract.EndDate
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Assign(BadgeAssignModel model)
        {
            cMgr.HandleBadgeAssignment(model.HolderId, model.NewMifareSerial, model.Start, model.End);
            return RedirectToAction("LijstActive", "Holder");
        }

        public ActionResult Activate(string badgeId)
        {
            Badge badge = cMgr.GetBadge(badgeId);
            cMgr.ChangeBadgeStatusToActive(badge);
            return RedirectToAction("LijstActive", "Holder");
        }

        public ActionResult Lost(string badgeId)
        {
            Badge badge = cMgr.GetBadge(badgeId);
            cMgr.ChangeBadgeStatusToLost(badge);
            return RedirectToAction("LijstActive", "Holder");
        }

        public ActionResult Disable(string badgeId)
        {
            Badge badge = cMgr.GetBadge(badgeId);
            cMgr.ChangeBadgeStatusToDisabled(badge);
            return RedirectToAction("LijstActive", "Holder");
        }
    }
}