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
        [HttpPost]
        public ActionResult Toevoegen(InputHolder nieuweHolder)
        {
            if (ModelState.IsValid)
            {
                mng.AddNewHolder(nieuweHolder);
            }
            return View();
        }

        public ActionResult Index()
        {
            return RedirectToAction("Lijst");
        }

        public ActionResult Toevoegen()
        {
            return View();
        }

        public ActionResult Lijst()
        {
            ViewData.Model = mng.GetHolders();
            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(404);
            }
            int newId = (int) id;
            ViewData.Model = mng.GetHolder(newId);
            if (ViewData.Model == null)
            {
                return new HttpStatusCodeResult(404);
            } else
            {
                return View();
            }
        }

        [HttpPut]
        public ActionResult Edit(int? id, InputHolder updateHolder)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(404);
            }
            int newId = (int)id;
            if (mng.GetHolder(newId) == null)
            {
                return new HttpStatusCodeResult(404);
            } else if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(500);
            }
            mng.ChangeHolder(newId, updateHolder);
            return new HttpStatusCodeResult(200);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(404);
            }
            int newId = (int)id;
            ViewData.Model = mng.GetHolder(newId);
            if (ViewData.Model == null)
            {
                return new HttpStatusCodeResult(404);
            }
            else
            {
                return View();
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(404);
            }
            int newId = (int)id;
            ViewData.Model = mng.GetHolder(newId);
            if (ViewData.Model == null)
            {
                return new HttpStatusCodeResult(404);
            }
            else
            {
                return View();
            }
        }
    }
}