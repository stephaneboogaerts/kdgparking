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
using System.Diagnostics;

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

        public ActionResult CompanyToevoegen(Company company)
        {
            if (ModelState.IsValid)
            {
                mng.AddCompany(CleanString(company.CompanyName));
                return new HttpStatusCodeResult(200);
            } else
            {
                return new HttpStatusCodeResult(500);
            }
        }

        public ActionResult Edit(int? id)
        {
            if (this.VerifyId(id))
            {
                int newId = (int)id;
                ViewData.Model = mng.GetCompany(newId);
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(404);
            }
        }

        public ActionResult EditCompany(int? id, Company company)
        {
            if (this.VerifyId(id))
            {
                int newId = (int)id;
                if (ModelState.IsValid)
                {
                    company.CompanyName = CleanString(company.CompanyName);
                }
                mng.UpdateCompany(company);
                return RedirectToAction("Lijst");
            }
            else
            {
                return new HttpStatusCodeResult(404);
            }
        }

        private bool VerifyId(int? id)
        {
            if (id == null)
            {
                return false;
            }
            else
            {
                int newId = (int)id;
                if (mng.GetCompany(newId) == null)
                {
                    return false;
                }
            }
            return true;
        }

        private string CleanString(string input)
        {
            string DirtyCharacters = "ŠŽšžŸÀÁÂÃÄÅÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖÙÚÛÜÝàáâãäåçèéêëìíîïðñòóôõöùúûüýÿ";
            string CleanCharacters = "SZszYAAAAAACEEEEIIIIDNOOOOOUUUUYaaaaaaceeeeiiiidnooooouuuuyy";
            for (int i = 0; i < DirtyCharacters.Length; i++)
            {
                char DirtyChar = DirtyCharacters[i];
                char CleanChar = CleanCharacters[i];
                input = input.Replace(DirtyChar, CleanChar);
            }
            return input;
        }
    }
}