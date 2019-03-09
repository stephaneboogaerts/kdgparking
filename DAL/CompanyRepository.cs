using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kdgparking.BL.Domain;
using kdgparking.DAL.EF;

namespace kdgparking.DAL
{
    public class CompanyRepository : ICompanyRepository
    {
        private OurDbContext ctx;

        public CompanyRepository()
        {
            ctx = new OurDbContext();
            ctx.Database.Initialize(false);
        }

        public Company CreateCompany(Company company)
        {
            ctx.Companies.Add(company);
            ctx.SaveChanges();

            return company;
        }

        public Company ReadCompany(string companyName)
        {
            Company company = ctx.Companies.FirstOrDefault(c => c.CompanyName == companyName);
            return company;
        }

        public List<Company> ReadCompanies()
        {
            List<Company> companies = ctx.Companies.ToList<Company>();
            return companies;
        }

        public List<Company> ReadCompanies(string searchString)
        {
            List<Company> companies = ctx.Companies.Where(c => (c.CompanyName.ToLower()).Contains(searchString.ToLower())).ToList<Company>();
            return companies;
        }

        public void Dispose()
        {
            ctx.Dispose();
        }
    }
}
