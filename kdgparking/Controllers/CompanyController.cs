using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kdgparking.BL;
using kdgparking.BL.Domain;
using System.Net.Mail;
using System.Threading.Tasks;
using kdgparking.Models;

namespace kdgparking.Controllers
{
    public class CompanyController : Controller
    {

        private ICompanyManager mng = new CompanyManager();

        public ActionResult Index()
        {
            return RedirectToAction("Lijst");
        }

        public ActionResult Lijst(string searchString)
        {
                IEnumerable<Company> companies;
                if (!String.IsNullOrEmpty(searchString))
                {
                    companies = mng.GetCompanies(searchString);
                }
                else
                {
                    companies = mng.GetCompanies();
                }
                ViewData.Model = companies;
                return View();
        }

        public ActionResult Toevoegen()
        {
            return View();
        }
    }
}