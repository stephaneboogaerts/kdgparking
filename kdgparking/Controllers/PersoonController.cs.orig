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

        public ActionResult Toevoegen()
        {
            return View();
        }
    }
}