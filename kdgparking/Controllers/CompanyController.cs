using kdgparking.BL;
using kdgparking.BL.Domain;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace kdgparking.Controllers
{
    public class CompanyController : Controller
    {

        private ICompanyManager mng = new CompanyManager();
        private StringCleaner cleaner = new StringCleaner();

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

        public ActionResult CompanyToevoegen(Company company)
        {
            if (ModelState.IsValid)
            {
                mng.AddCompany(cleaner.CleanString(company.CompanyName));
                return new HttpStatusCodeResult(200);
            } else
            {
                return new HttpStatusCodeResult(500);
            }
        }

        public ActionResult Edit(int? inputId)
        {
            int id = VerifyId(inputId);
            if (id != -1) 
            {
                ViewData.Model = mng.GetCompany(id);
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(404);
            }
        }

        public ActionResult EditCompany(int? inputId, Company company)
        {
            int id = VerifyId(inputId);
            if (id != -1)
            { 
                if (ModelState.IsValid)
                {
                    company.CompanyName = cleaner.CleanString(company.CompanyName);
                }
                mng.UpdateCompany(company);
                return RedirectToAction("Lijst");
            }
            else
            {
                return new HttpStatusCodeResult(404);
            }
        }

        private int VerifyId(int? id)
        {
            if (id == null)
            {
                return -1;
            }
            else if (mng.GetCompany((int)id) == null)
            {
                return -1;
            }
            return (int)id;
        }
    }
}