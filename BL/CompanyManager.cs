using kdgparking.BL.Domain;
using kdgparking.DAL;
using kdgparking.DAL.EF;
using System.Collections.Generic;

namespace kdgparking.BL
{
    public class CompanyManager : ICompanyManager
    {
        private readonly ICompanyRepository repo;

        public CompanyManager()
        {
            repo = new CompanyRepository();
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

        public Company GetCompany(int id)
        {
            return repo.ReadCompany(id);
        }

        public List<Company> GetCompanies()
        {
            return repo.ReadCompanies();
        }

        public List<Company> GetCompanies(string searchString)
        {
            return repo.ReadCompanies(searchString);
        }

        public Company UpdateCompany(Company company)
        {
            return repo.UpdateCompany(company);
        }

        // Kijken of een Company alreeds bestaat in de DB
        // Wanneer de Company niet gevonden word, wordt er een nieuwe Company aangemaakt
        public Company CheckAndCreateCompany(string companyName)
        {
            Company company = GetCompany(companyName);
            if (company == null)
            {
                company = AddCompany(companyName);
            }
            return company;
        }

        private Company AddCompany(Company company)
        {
            return repo.CreateCompany(company);
        }
    }
}
