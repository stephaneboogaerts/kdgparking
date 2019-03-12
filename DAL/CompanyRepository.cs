using kdgparking.BL.Domain;
using kdgparking.DAL.EF;
using System.Collections.Generic;
using System.Linq;

namespace kdgparking.DAL
{
    public class CompanyRepository : ICompanyRepository
    {
        public OurDbContext ctx { get; }

        public CompanyRepository()
        {
            ctx = new OurDbContext();
            ctx.Database.Initialize(false);
        }

        public CompanyRepository(OurDbContext context)
        {
            ctx = context;
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

        public Company ReadCompany(int id)
        {
            Company company = ctx.Companies.FirstOrDefault(c => c.CompanyId == id);
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

        public Company UpdateCompany(Company company)
        {
            ctx.Entry(company).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
            return company;
        }

        public void Dispose()
        {
            ctx.Dispose();
        }
    }
}
