using kdgparking.BL;
using System.Web.Mvc;

namespace kdgparking.Controllers
{
    public class HomeController : Controller
    {
        private IHolderManager mng = new HolderManager();

        public ActionResult Index()
        {
            //Vraag van klant, alles van Excel op de homepagina
            return RedirectToAction("Index", "Excel");
        }

        //NIET FUNCTIONEEL: Doel was om TGC view hier ook te tonen
        public ActionResult SQL()
        {
            mng.GetSQLView();
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}