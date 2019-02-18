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

        public ActionResult voegPersoonToe(string naam, string voornaam, DateTime start, DateTime end, string email, string company, string nummerplaat)
        {
            Person nieuwePersoon;
            try
            {
                nieuwePersoon = new Person(naam, voornaam, start, end, company, new MailAddress(email), nummerplaat);
                Console.WriteLine("Created new Person");
            } catch
            {
                Console.WriteLine("Failed to create new Person");
                return new HttpStatusCodeResult(500);
            }
            mng.AddPersoon(nieuwePersoon);
            return new HttpStatusCodeResult(200);
        }

        public ActionResult Toevoegen()
        {
            return View();
        }
    }
}