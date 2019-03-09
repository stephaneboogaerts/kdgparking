using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using kdgparking.DAL;
using kdgparking.BL.Domain;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Web;
using OfficeOpenXml;
using System.IO;
using Syroot.Windows.IO;
using kdgparking.DAL.EF;

namespace kdgparking.BL
{
    public class CompanyManager : ICompanyManager
    {
        private readonly ICompanyRepository repo;

        public CompanyManager()
        {
            repo = new kdgparking.DAL.CompanyRepository();
        }

        public CompanyManager(OurDbContext context)
        {
            repo = new CompanyRepository(context);
        }

        public Company AddCompany(string companyName)
        {
            Company company = new Company()
            {
                CompanyName = companyName
            };
            return this.AddCompany(company);
        }

        public Company GetCompany(string companyName)
        {
            return repo.ReadCompany(companyName);
        }

        public List<Company> GetCompanies()
        {
            return repo.ReadCompanies();
        }

        public List<Company> GetCompanies(string searchString)
        {
            return repo.ReadCompanies(searchString);
        }

        private Company AddCompany(Company company)
        {
            // Validatie gebeurt in InputHolder
            return repo.CreateCompany(company);
        }

    }
}
