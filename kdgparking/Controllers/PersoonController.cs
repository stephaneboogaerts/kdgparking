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

        public ActionResult voegPersoonToe(string naam, string voornaam, DateTime start, DateTime end, string email, string company, string nummerplaat)
        {
            Holder nieuweHolder;
            try
            {
                //nieuwePersoon = new Persoon(naam, voornaam, start, end, company, new MailAddress(email), nummerplaat);
                nieuweHolder = mng.AddHolder("UniekeString001", naam, voornaam, tmpPhone, email);
                Console.WriteLine("Created new Person");
            } catch
            {
                Console.WriteLine("Failed to create new Person");
                return new HttpStatusCodeResult(500);
            }
            //mng.AddHolder(nieuwePersoon);
            return new HttpStatusCodeResult(200);
        }

        public ActionResult Toevoegen()
        {
            return View();
        }
    }
}