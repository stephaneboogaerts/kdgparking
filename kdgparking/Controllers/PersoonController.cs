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
        public int tmpPhone = 031112233;

        [HttpPost]
        public ActionResult HolderToevoegen(InputHolder nieuweHolder)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                mng.createNewHolder(nieuweHolder);
                return new HttpStatusCodeResult(200);
            }
        }

        public ActionResult HolderToevoegen()
        {
            return View();
        }
    }
}