using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kdgparking.BL;
using kdgparking.BL.Domain;
using System.Net.Mail;

namespace testParkingWeb.Controllers
{
    public class PersoonController : Controller
    {
        private IManager mng = new Manager();

        //Meer feedback

        public ActionResult Index()
        {
            return RedirectToAction("Lijst");
        }

        public ActionResult Lijst()
        {
            IEnumerable<Holder> holders = mng.GetHolders();
            List<InputHolder> iHolders = new List<InputHolder>();
            foreach (Holder h in holders)
            {
                iHolders.Add(this.ComposeInputHolder(h));
            }
            ViewData.Model = iHolders;
            return View();
        }

        public ActionResult Toevoegen()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Toevoegen(InputHolder nieuweHolder)
        {
            if (ModelState.IsValid)
            {
                mng.AddNewHolder(nieuweHolder);
            }
            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (this.VerifyId(id))
            {
                int newId = (int)id;
                ViewData.Model = this.ComposeInputHolder(mng.GetHolder(newId));
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(404);
            }
        }

        public ActionResult EditHolder(int? id, InputHolder updateHolder)
        {
            if (!this.VerifyId(id))
            {
                return new HttpStatusCodeResult(404);
            } else if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(500);
            }
            int newId = (int)id;
            mng.UpdateHolder(newId, updateHolder);
            return RedirectToAction("Lijst");
        }

        public ActionResult Details(int? id)
        {
            if (this.VerifyId(id))
            {
                int newId = (int)id;
                ViewData.Model = this.ComposeInputHolder(mng.GetHolder(newId));
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(404);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (this.VerifyId(id))
            {
                int newId = (int)id;
                ViewData.Model = this.ComposeInputHolder(mng.GetHolder(newId));
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(404);
            }
        }

        private InputHolder ComposeInputHolder(Holder holder)
        {
            Contract holderContract = mng.GetHolderContract(holder.Id);
            return new InputHolder()
            {
                HolderId = holder.Id,
                Name = holder.Name,
                FirstName = holder.FirstName,
                Company = holder.Company.CompanyName,
                Email = holder.Email,
                StartDate = holderContract.StartDate,
                EndDate = holderContract.EndDate,
            };
        }

        private bool VerifyId(int? id)
        {
            if (id == null)
            {
                return false;
            } else
            {
                int newId = (int)id;
                if (mng.GetHolder(newId) == null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}