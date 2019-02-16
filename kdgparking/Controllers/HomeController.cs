using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using kdgparking.BL;
using kdgparking.BL.Domain;

namespace kdgparking.Controllers
{
    public class HomeController : Controller
    {
        private IManager mgr = new Manager();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CRUD()
        {
            ViewBag.Message = "CRUD page";

            return View();
        }
    }
}