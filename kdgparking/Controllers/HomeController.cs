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
        private IHolderManager mng = new HolderManager();

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Excel");
        }

        public ActionResult SQL()
        {
            mng.GetSQLView();
            return View();
        }
    }
}